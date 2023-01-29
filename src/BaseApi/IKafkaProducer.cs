using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.BaseSerializers;

namespace Kafka.BaseApi
{
    public interface IKafkaProducer<K, V>
    {
        IProducer<K, V> ProducerObject { get; }

        void SetRecordSerializer(IKafkaSerializer serializer);
        bool ExistsTopic(string topicName);
        Task CreateTopicAsync(string topicName, int numPartitions, int replicationFactor = 3);
        Task CreateTopicAsync(TopicSpecification topicSpecification, CreateTopicsOptions? options = null);
        Task DeleteTopicAsync(string topicName, DeleteTopicsOptions? options = null);
        Task<DeliveryResult<K, V>> ProduceAsync(string topic, V value);
        Task<DeliveryResult<K, V>> ProduceAsync(string topic, K key, V value);
        Task<DeliveryResult<K, V>> ProduceAsync(string topic, Message<K, V> message);
        void Produce(string topic, V value, Action<DeliveryReport<K, V>>? deliveryHandler = null);
        void Produce(string topic, K key, V value, Action<DeliveryReport<K, V>>? deliveryHandler = null);
        void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>>? deliveryHandler = null);
        void Flush(TimeSpan timeout);
        void FlushAll();
        void Dispose();
    }
}