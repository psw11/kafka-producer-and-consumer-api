using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Kafka.CustomConfig;
using Kafka.CustomApi;

namespace Kafka.Workers
{
    internal class WorkerWithConfigTopic
    {
        private readonly IConfiguration _configuration;
        private readonly KafkaStringManualProducer _kafkaProducer;
        private readonly KafkaStringManualConsumer _kafkaConsumer;

        public WorkerWithConfigTopic(
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
            var topicName = _configuration.GetValue<string>("KafkaManualCommit:Topics:RequestTopicName");

            Task.Run(async () =>
            {
                if (!_kafkaProducer.ExistsTopic(topicName))
                {
                    await _kafkaProducer.CreateTopicAsync(new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = 2,
                        ReplicationFactor = 3,                                  // Количество реплик разделов топика
                        Configs = new Dictionary<string, string>
                            {
                                { "min.insync.replicas", "2" },                 // Минимальное количество синхронных реплик раздела
                                { "unclean.leader.election.enable", "false" },  // Выбор рассинхронизированной реплики в качестве ведущей

                                { "segment.bytes", "100024" },                  // (100 Кб) Определяет максимальный размер файла сегмента журнала
                                { "cleanup.policy", "delete" },                 // Включает удаление старых сегментов журнала
                                { "retention.bytes", "-1" },                    // Определяет размер партиции, после превышения которого следует начать удалять сообщения
                                { "retention.ms", "600000" },                   // (10 минут) Определяет максимальное время хранения сегментов журнала, по истечении которого расширение файлов изменится на *.deleted
                                { "file.delete.delay.ms", "60000" }             // (1 минута) Определяет время задержки перед удалением файлов с расширением *.deleted с диска

                                // { "cleanup.policy", "compact" }                      // Включает сжатие журнала, когда сохраняется только уникальное значение для каждого ключа. Если указать "compact, delete", то старые сегменты будут удалены, а оставшиеся - сжаты. 
                                // { "max.compaction.lag.ms", "9223372036854775807" }   // Максимальное время, в течение которого сообщение не может быть сжато в журнале
                            }
                    });
                }

                for (var i = 0; i < 10000; i++)
                {
                    var deliveryResult = await _kafkaProducer.ProduceAsync(topicName!, i.ToString());
                    PostProducingMessage(topicName, i);
                }

                _kafkaProducer.FlushAll();
            });
        }

        public void RunConsume()
        {
            var topicName = _configuration.GetValue<string>("KafkaManualCommit:Topics:RequestTopicName");

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
