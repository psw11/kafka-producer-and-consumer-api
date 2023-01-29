namespace Kafka.BaseConfig
{
    using KafkaSettings = IEnumerable<KeyValuePair<string, string>>;

    public interface IKafkaConfig
    {
        KafkaSettings GetAdminConfig();
        KafkaSettings GetConsumerConfig();
        KafkaSettings GetProducerConfig();
        KafkaSettings GetSchemaRegistryConfig();
    }
}