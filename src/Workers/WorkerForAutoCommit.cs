using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Kafka.CustomConfig;
using Kafka.CustomApi;

namespace Kafka.Workers
{
    internal class WorkerForAutoCommit
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaStringAutoProducer _kafkaProducer;
        private readonly KafkaStringAutoConsumer _kafkaConsumer;

        public WorkerForAutoCommit(
            IConfiguration configuration,
            KafkaStringAutoProducer kafkaProducer,
            KafkaStringAutoConsumer kafkaConsumer)
        {
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
            _kafkaConsumer = kafkaConsumer;
        }

        public void RunProduce()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName");

            Task.Run(async () =>
            {
                if (!_kafkaProducer.ExistsTopic(topicName))
                {
                    await _kafkaProducer.CreateTopicAsync(topicName, 1, 3);
                }

                for (var i = 0; i < 10000; i++)
                {
                    await _kafkaProducer.ProduceAsync(topicName!, i.ToString());
                    PostProducingMessage(topicName, i);
                }

                _kafkaProducer.FlushAll();
            });
        }

        public void RunConsume()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName");

            if (!_kafkaConsumer.ExistsTopic(topicName))
            {
                Console.WriteLine($"Topic {topicName} not found");
                return;
            }

            _kafkaConsumer.StartConsuming(topicName);
            _kafkaConsumer.PerformMessage += (consumer, consumeResult) =>
            {
                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine("End of messages. Wait for new");
                    return;
                }

                PerformMessage(consumeResult);

                // Store the offset associated with consumeResult to a local cache
                // Enable that, if "enable.auto.offset.store" = false
                // consumer.StoreOffset(consumeResult);
            };

            _kafkaConsumer.PerformError += (error) =>
            {
                return PerformError(error);
            };
        }

        private static void PostProducingMessage(string? topicName, int i)
        {
            Console.WriteLine("Sent message to " +
                $"topic `{topicName}`, " +
                $"message: `` = `{i}`");

            //Task.Delay(100).Wait();
        }

        private void PerformMessage(ConsumeResult<string, string> consumeResult)
        {
            Console.WriteLine($"Received message from " +
                $"topic `{consumeResult.Topic}`, " +
                $"partition `{consumeResult.Partition}`, " +
                $"offset `{consumeResult.Offset}` " +
                $"message: `{consumeResult.Message.Key}` = `{consumeResult.Message.Value}`");

            //Task.Delay(100).Wait();
        }

        private bool PerformError(Exception error)
        {
            Console.WriteLine(error);

            if (error is ConsumeException consumeException)
            {
                // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                if (consumeException.Error.IsFatal)
                {
                    return true;    // Stop consume
                }
            }

            return false;
        }
    }
}
