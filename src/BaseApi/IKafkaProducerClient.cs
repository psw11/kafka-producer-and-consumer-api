using Confluent.Kafka;
using Kafka.BaseConfig;

namespace Kafka.BaseApi
{
    public interface IKafkaProducerClient
    {
        IAdminClient KafkaAdmin { get; }
        IProducer<byte[], byte[]> KafkaDefaultProducer { get; }

        void Dispose();
    }

    public interface IKafkaProducerClient<TKafkaConfig> : IKafkaProducerClient
        where TKafkaConfig : IKafkaConfig
    {
    }
}