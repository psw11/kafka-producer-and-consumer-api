using Microsoft.Extensions.Configuration;

namespace Kafka.BaseConfig
{
    using KafkaSettings = IEnumerable<KeyValuePair<string, string>>;

    public class KafkaConfig : IKafkaConfig
    {
        protected virtual string KafkaRootSectionName => "Kafka";
        private readonly IConfiguration _config;

        public KafkaConfig(IConfiguration config)
        {
            _config = config;
        }

        public KafkaSettings GetAdminConfig()
        {
            return GetConfigBySections(
                $"{KafkaRootSectionName}:BrokerSettings");
        }

        public KafkaSettings GetProducerConfig()
        {
            return GetConfigBySections(
                $"{KafkaRootSectionName}:BrokerSettings", 
                $"{KafkaRootSectionName}:ProducerSettings");
        }

        public KafkaSettings GetConsumerConfig()
        {
            return GetConfigBySections(
                $"{KafkaRootSectionName}:BrokerSettings", 
                $"{KafkaRootSectionName}:ConsumerSettings");
        }

        public KafkaSettings GetSchemaRegistryConfig()
        {
            return GetConfigBySections(
                $"{KafkaRootSectionName}:SchemaRegistrySettings");
        }

        private KafkaSettings GetConfigBySections(params string[] sectionsNames)
        {
            var sectionsProperties = sectionsNames
                .SelectMany(sectionName => _config.GetSection(sectionName).GetChildren()
                    .Where(e => !string.IsNullOrWhiteSpace(e.Value))
                    .Select(e => new KeyValuePair<string, string>(e.Key, e.Value!))
                );

            if (!sectionsProperties.Any())
                throw new Exception("Config settings is empty");

            return sectionsProperties;
        }
    }
}
