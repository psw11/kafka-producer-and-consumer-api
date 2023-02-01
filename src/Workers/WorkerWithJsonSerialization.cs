using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Kafka.CustomApi;
using Kafka.Messages;
using Kafka.CustomSerializers;

namespace Kafka.Workers
{
    internal class WorkerWithJsonSerialization
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaJsonAutoProducer _kafkaProducer;
        private readonly KafkaJsonAutoConsumer _kafkaConsumer;

        public WorkerWithJsonSerialization(
            IConfiguration configuration,
            KafkaJsonAutoProducer kafkaProducer,
            KafkaJsonAutoConsumer kafkaConsumer,
            KafkaJsonSerializer jsonSerializer)
        {
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
            _kafkaConsumer = kafkaConsumer;

            kafkaProducer.SetRecordSerializer(jsonSerializer);
            kafkaConsumer.SetRecordSerializer(jsonSerializer);
        }

        public void RunProduce()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName") + "6";

            Task.Run(async () =>
            {
                try
                {
                    if (!_kafkaProducer.ExistsTopic(topicName))
                    {
                        await _kafkaProducer.CreateTopicAsync(topicName, 1, 3);
                    }

                    for (var i = 0; i < 10000; i++)
                    {
                        var message = new KafkaJsonMessage()
                        {
                            TypeName = "MessageType",
                            TypeVersion = "v1",
                            Data = i
                        };

                        await _kafkaProducer.ProduceAsync(topicName!, message);
                        PostProducingMessage(topicName, message);
                    }

                    _kafkaProducer.FlushAll();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        public void RunConsume()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName") + "6";

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
            };

            _kafkaConsumer.PerformError += (error) =>
            {
                return PerformError(error);
            };
        }

        private static void PostProducingMessage(string? topicName, KafkaJsonMessage message)
        {
            Console.WriteLine("Sent message to " +
                $"topic `{topicName}`, " +
                $"message: `` = `{message}`");

            //Task.Delay(100).Wait();
        }

        private void PerformMessage(ConsumeResult<string, KafkaJsonMessage> consumeResult)
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
