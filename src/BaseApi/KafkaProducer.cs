using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.BaseSerializers;

namespace Kafka.BaseApi
{
    public class KafkaProducer<K, V> : IDisposable, IKafkaProducer<K, V>
    {
        private IKafkaProducerClient _kafkaClient;
        private IKafkaSerializer? _kafkaServializer;

        private IAdminClient _kafkaAdmin;
        private IProducer<K, V>? _kafkaProducer;
        private IProducer<K, V> KafkaProducerInt => _kafkaProducer ?? CreateKafkaProducer();

        public IProducer<K, V> ProducerObject => KafkaProducerInt;
        public readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(60);
        public const int DefaultTopicReplicationFactor = 3;             // Minimum value of replication

        public KafkaProducer(IKafkaProducerClient kafkaClient)
        {
            _kafkaClient = kafkaClient;
            _kafkaAdmin = kafkaClient.KafkaAdmin;
        }

        private IProducer<K, V> CreateKafkaProducer()
        {
            var producerBuilder = new DependentProducerBuilder<K, V>(_kafkaClient.KafkaDefaultProducer.Handle);

            if (_kafkaServializer != null)
            {
                producerBuilder.SetValueSerializer(_kafkaServializer.GetValueSerializer<V>());
            }

            return _kafkaProducer = producerBuilder.Build();
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

        public async Task CreateTopicAsync(string topicName, int numPartitions, int replicationFactor = DefaultTopicReplicationFactor)
        {
            var clusterMetadata = _kafkaAdmin.GetMetadata(DefaultRequestTimeout);
            var brokersCount = (short)clusterMetadata.Brokers.Count();

            var topicSpecification = new TopicSpecification
            {
                Name = topicName,
                NumPartitions = numPartitions,
                ReplicationFactor = Math.Min(brokersCount, (short)replicationFactor),
            };

            await CreateTopicAsync(topicSpecification);
        }

        public async Task CreateTopicAsync(TopicSpecification topicSpecification, CreateTopicsOptions? options = null)
        {
            await _kafkaAdmin.CreateTopicsAsync(new[] { topicSpecification }, options);
        }

        public async Task DeleteTopicAsync(string topicName, DeleteTopicsOptions? options = null)
        {
            await _kafkaAdmin.DeleteTopicsAsync(new[] { topicName }, options);
        }

        public async Task<DeliveryResult<K, V>> ProduceAsync(string topic, V value)
        {
            return await ProduceAsync(topic, default, value);
        }

        public async Task<DeliveryResult<K, V>> ProduceAsync(string topic, K key, V value)
        {
            var message = new Message<K, V>
            {
                Key = key,
                Value = value,
                Headers = default,
                Timestamp = new Timestamp(DateTime.UtcNow)
            };

            return await ProduceAsync(topic, message);
        }

        public async Task<DeliveryResult<K, V>> ProduceAsync(string topic, Message<K, V> message)
        {
            return await KafkaProducerInt.ProduceAsync(topic, message);
        }

        public void Produce(string topic, V value, Action<DeliveryReport<K, V>>? deliveryHandler = null)
        {
            Produce(topic, default, value, deliveryHandler);
        }

        public void Produce(string topic, K key, V value, Action<DeliveryReport<K, V>>? deliveryHandler = null)
        {
            var message = new Message<K, V>
            {
                Key = key,
                Value = value,
                Headers = default,
                Timestamp = new Timestamp(DateTime.UtcNow)
            };

            Produce(topic, message, deliveryHandler);
        }

        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>>? deliveryHandler = null)
        {
            KafkaProducerInt.Produce(topic, message, deliveryHandler);
        }

        public void Flush(TimeSpan timeout)
        {
            KafkaProducerInt.Flush(timeout);
        }

        public void FlushAll()
        {
            KafkaProducerInt.Flush();
        }

        public void Dispose()
        {
            KafkaProducerInt.Flush();       // Delivery messages completely
            KafkaProducerInt.Dispose();

            _kafkaAdmin.Dispose();
            _kafkaServializer?.Dispose();
        }
    }
}
