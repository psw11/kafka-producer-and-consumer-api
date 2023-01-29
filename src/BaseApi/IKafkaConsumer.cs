using Confluent.Kafka;
using Kafka.BaseSerializers;

namespace Kafka.BaseApi
{
    public interface IKafkaConsumer<K, V>
    {
        IConsumer<K, V> ConsumerObject { get; }

        event PerformExceptionHandler? PerformError;
        event PerformMessageHandler<K, V>? PerformMessage;

        void SetRecordSerializer(IKafkaSerializer serializer);
        bool ExistsTopic(string topicName);
        WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition);
        WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition, TimeSpan timeout);
        void StartConsuming(params string[] topicsNames);
        void StartConsuming(string[] topicsNames, CancellationToken cancellationToken);
        void StartConsuming(params TopicPartitionOffset[] topicPartitions);
        void StartConsuming(TopicPartitionOffset[] topicPartitions, CancellationToken cancellationToken);
        void Dispose();
    }
}