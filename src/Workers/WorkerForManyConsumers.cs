using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Kafka.CustomConfig;
using Kafka.CustomApi;

namespace Kafka.Workers
{
    internal class WorkerForManyConsumers
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaStringAutoProducer _kafkaProducer;
        private readonly KafkaStringAutoConsumer _kafkaAutoConsumer;
        private readonly KafkaStringManualConsumer _kafkaManualConsumer;

        public WorkerForManyConsumers(
            IConfiguration configuration,
            KafkaStringAutoProducer kafkaProducer,
            KafkaStringAutoConsumer kafkaAutoConsumer,
            KafkaStringManualConsumer kafkaManualConsumer)
        {
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
            _kafkaAutoConsumer = kafkaAutoConsumer;
            _kafkaManualConsumer = kafkaManualConsumer;
        }

        public void RunProduce()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName") ?? "";

            Task.Run(async () =>
            {
                if (!_kafkaProducer.ExistsTopic(topicName))
                {
                    await _kafkaProducer.CreateTopicAsync(topicName, 2, 3);
                }

                for (var i = 0; i < 10000; i++)
                {
                    await _kafkaProducer.ProduceAsync(topicName!, i.ToString());
                    //await Task.Delay(100);
                }

                _kafkaProducer.FlushAll();
            });
        }

        public void RunAutoConsume()
        {
            var groupId = _configuration.GetValue<string>("KafkaAutoCommit:ConsumerSettings:group.id") ?? "";
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName") ?? "";

            if (!_kafkaAutoConsumer.ExistsTopic(topicName))
            {
                Console.WriteLine($"Topic {topicName} not found");
                return;
            }

            _kafkaAutoConsumer.StartConsuming(topicName);
            _kafkaAutoConsumer.PerformMessage += (consumer, consumeResult) =>
            {
                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine("End of messages. Wait for new");
                    return;
                }

                PerformMessage(groupId, consumeResult);
            };

            _kafkaAutoConsumer.PerformError += (error) =>
            {
                return PerformError(error);
            };
        }

        public void RunManualConsume()
        {
            var groupId = _configuration.GetValue<string>("KafkaManualCommit:ConsumerSettings:group.id") ?? "";
            var topicName = _configuration.GetValue<string>("KafkaManualCommit:Topics:RequestTopicName") ?? "";

            if (!_kafkaManualConsumer.ExistsTopic(topicName))
            {
                Console.WriteLine($"Topic {topicName} not found");
                return;
            }

            _kafkaManualConsumer.StartConsuming(topicName);
            _kafkaManualConsumer.PerformMessage += (consumer, consumeResult) =>
            {
                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine("End of messages. Wait for new");
                    return;
                }

                PerformMessage(groupId, consumeResult);

                // Commit the offset to the broker
                consumer.Commit(consumeResult);
            };

            _kafkaManualConsumer.PerformError += (error) =>
            {
                return PerformError(error);
            };
        }

        private void PerformMessage(string groupId, ConsumeResult<string, string> consumeResult)
        {
            Console.WriteLine($"Received message from " +
                $"topic `{consumeResult.Topic}`, " +
                $"partition `{consumeResult.Partition}`, " +
                $"groupId `{groupId}` " +
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
