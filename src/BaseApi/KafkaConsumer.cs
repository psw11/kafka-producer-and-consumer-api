using Confluent.Kafka;
using Kafka.BaseConfig;
using Kafka.BaseSerializers;

namespace Kafka.BaseApi
{
    public delegate void PerformMessageHandler<K, V>(IConsumer<K, V> consumer, ConsumeResult<K, V> consumeResult);
    public delegate bool PerformExceptionHandler(Exception error);

    public class KafkaConsumer<K, V> : IDisposable, IKafkaConsumer<K, V>
    {
        private readonly IKafkaConfig _kafkaConfig;
        private IKafkaSerializer? _kafkaServializer;

        private readonly IAdminClient _kafkaAdmin;
        private IConsumer<K, V>? _kafkaConsumer;
        private IConsumer<K, V> KafkaConsumerInt => _kafkaConsumer ?? CreateKafkaConsumer();

        public IConsumer<K, V> ConsumerObject => KafkaConsumerInt;
        public readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(60);
        public event PerformMessageHandler<K, V>? PerformMessage;
        public event PerformExceptionHandler? PerformError;

        public KafkaConsumer(IKafkaConfig kafkaConfig)
        {
            _kafkaConfig = kafkaConfig;

            var adminConfig = kafkaConfig.GetAdminConfig();
            _kafkaAdmin = new AdminClientBuilder(adminConfig).Build();
        }

        private IConsumer<K, V> CreateKafkaConsumer()
        {
            var consumerConfig = _kafkaConfig.GetConsumerConfig();
            var consumerBuilder = new ConsumerBuilder<K, V>(consumerConfig);

            if (_kafkaServializer != null)
            {
                consumerBuilder.SetValueDeserializer(_kafkaServializer.GetValueDeserializer<V>());
            }

            return _kafkaConsumer = consumerBuilder.Build();
        }

        public void SetRecordSerializer(IKafkaSerializer serializer)
        {
            _kafkaServializer = serializer;
        }

        public bool ExistsTopic(string topicName)
        {
            var clusterMetadata = _kafkaAdmin.GetMetadata(DefaultRequestTimeout);
            var correctTopicName = topicName.ToLower().Trim();

            return clusterMetadata.Topics
                .Any(t => t.Topic.ToLower().Trim() == correctTopicName);
        }

        public WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition)
        {
            return GetWatermarkOffsets(topicPartition, DefaultRequestTimeout);
        }

        public WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition, TimeSpan timeout)
        {
            return KafkaConsumerInt.QueryWatermarkOffsets(topicPartition, timeout);
        }

        public void StartConsuming(params string[] topicsNames)
        {
            StartConsuming(topicsNames, CancellationToken.None);
        }

        public void StartConsuming(string[] topicsNames, CancellationToken cancellationToken)
        {
            Task.Run(() =>
                {
                    StartConsumingInt(topicsNames, cancellationToken);
                },
                cancellationToken);
        }

        private void StartConsumingInt(string[] topicsNames, CancellationToken cancellationToken)
        {
            KafkaConsumerInt.Subscribe(topicsNames);

            try
            {
                StartConsumingLoop(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                KafkaConsumerInt.Close();
            }
        }

        public void StartConsuming(params TopicPartitionOffset[] topicPartitions)
        {
            StartConsuming(topicPartitions, CancellationToken.None);
        }

        public void StartConsuming(TopicPartitionOffset[] topicPartitions, CancellationToken cancellationToken)
        {
            Task.Run(() =>
                {
                    StartConsumingInt(topicPartitions, cancellationToken);
                },
                cancellationToken);
        }

        private void StartConsumingInt(TopicPartitionOffset[] topicPartitions, CancellationToken cancellationToken)
        {
            KafkaConsumerInt.Assign(topicPartitions);

            try
            {
                StartConsumingLoop(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                KafkaConsumerInt.Close();
            }
        }

        private void StartConsumingLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = KafkaConsumerInt.Consume(cancellationToken);

                    PerformMessage?.Invoke(KafkaConsumerInt, consumeResult);
                }
                catch (Exception e)
                {
                    var stopConsuming = PerformError?.Invoke(e) ?? false;
                    if (stopConsuming)
                    {
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            KafkaConsumerInt.Close();     // Commit offsets and leave the group cleanly.
            KafkaConsumerInt.Dispose();

            _kafkaAdmin.Dispose();
            _kafkaServializer?.Dispose();
        }
    }
}
