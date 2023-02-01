using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Kafka.CustomApi;

namespace Kafka.Workers
{
    internal class WorkerForManualReadPartition
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaStringManualProducer _kafkaProducer;
        private readonly KafkaStringManualConsumer _kafkaConsumer;

        public WorkerForManualReadPartition(
            IConfiguration configuration,
            KafkaStringManualProducer kafkaProducer,
            KafkaStringManualConsumer kafkaConsumer)
        {
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
            _kafkaConsumer = kafkaConsumer;
        }

        public void RunProduce()
        {
            var topicName = _configuration.GetValue<string>("KafkaManualCommit:Topics:RequestTopicName") + "4";

            Task.Run(async () =>
            {
                if (!_kafkaProducer.ExistsTopic(topicName))
                {
                    await _kafkaProducer.CreateTopicAsync(topicName, 1, 3);
                }

                var lastValue = GetTopicLastValue(topicName);

                for (var i = lastValue + 1; i < lastValue + 1000; i++)
                {
                    await _kafkaProducer.ProduceAsync(topicName!, i.ToString());
                    PostProducingMessage(topicName, i);
                }

                _kafkaProducer.FlushAll();
            });
        }

        private int GetTopicLastValue(string? topicName)
        {
            var watermarkOffset = _kafkaConsumer.GetWatermarkOffsets(new TopicPartition(topicName, 0));
            
            if (watermarkOffset.High > 0)
            {
                var partitionOffset = new Offset(watermarkOffset.High.Value - 1);
                var partitionNumber = 0;
                var partitions = new[]
                {
                    new TopicPartitionOffset(topicName, partitionNumber, partitionOffset)
                };

                var cancellationTokenSource = new CancellationTokenSource();
                var result = default(int);

                _kafkaConsumer.StartConsuming(partitions, cancellationTokenSource.Token);
                _kafkaConsumer.PerformMessage += (consumer, consumeResult) =>
                {
                    if (consumeResult.IsPartitionEOF)
                    {
                        return;
                    }

                    if (int.TryParse(consumeResult?.Value, out var intValue))
                    {
                        result = intValue;
                    }
                    cancellationTokenSource.Cancel();
                };

                cancellationTokenSource.Token.WaitHandle.WaitOne();
                return result;
            }

            return 0;
        }

        public void RunConsume()
        {
            var topicName = _configuration.GetValue<string>("KafkaManualCommit:Topics:RequestTopicName") + "4";

            if (!_kafkaConsumer.ExistsTopic(topicName))
            {
                Console.WriteLine($"Topic {topicName} not found");
                return;
            }

            var partitionOffset = new Offset(1);    // or Offset.Beginning
            var partitionNumber = 0;
            var partitions = new[]
            {
                new TopicPartitionOffset(topicName, partitionNumber, partitionOffset)
            };

            _kafkaConsumer.StartConsuming(partitions);
            _kafkaConsumer.PerformMessage += (consumer, consumeResult) =>
            {
                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine("End of messages. Wait for new");
                    return;
                }

                PerformMessage(consumeResult);

                // Commit the offset to the broker
                consumer.Commit(consumeResult);
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
