using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Kafka.CustomApi;
using Kafka.Messages;
using Kafka.CustomSerializers;

namespace Kafka.Workers
{
    internal class WorkerWithAvroSerialization
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaAvroAutoProducer _kafkaProducer;
        private readonly KafkaAvroAutoConsumer _kafkaConsumer;

        public WorkerWithAvroSerialization(
            IConfiguration configuration,
            KafkaAvroAutoProducer kafkaProducer,
            KafkaAvroAutoConsumer kafkaConsumer,
            KafkaAvroSerializer avroSerializer)
        {
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
            _kafkaConsumer = kafkaConsumer;

            kafkaProducer.SetRecordSerializer(avroSerializer);
            kafkaConsumer.SetRecordSerializer(avroSerializer);
        }

        public void RunProduce()
        {
            var topicName = _configuration.GetValue<string>("KafkaAutoCommit:Topics:RequestTopicName");

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
                        var message = new KafkaAvroMessage()
                        {
                            TypeName = "MessageType",
                            TypeVersion = "v1",
                            Data = i.ToString()
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
            };

            _kafkaConsumer.PerformError += (error) =>
            {
                return PerformError(error);
            };
        }

        private static void PostProducingMessage(string? topicName, KafkaAvroMessage message)
        {
            Console.WriteLine("Sent message to " +
                $"topic `{topicName}`, " +
                $"message: `` = `{message}`");

            //Task.Delay(100).Wait();
        }

        private void PerformMessage(ConsumeResult<string, KafkaAvroMessage> consumeResult)
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
