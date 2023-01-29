using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Kafka.BaseConfig;

namespace Kafka.BaseSerializers
{
    public abstract class KafkaSerializer : IKafkaSerializer
    {
        private readonly IKafkaConfig _kafkaConfig;
        private CachedSchemaRegistryClient? _schemaRegistry;
        protected CachedSchemaRegistryClient SchemaRegistry => _schemaRegistry ?? CreateSchemaRegistry();

        public KafkaSerializer(IKafkaConfig kafkaConfig)
        {
            _kafkaConfig = kafkaConfig;
        }

        private CachedSchemaRegistryClient CreateSchemaRegistry()
        {
            var schemaRegistryConfig = _kafkaConfig.GetSchemaRegistryConfig();
            return _schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
        }

        protected R CreateGenericType<V, R>(Type sourceType, params object?[]? args)
        {
            var type = sourceType.MakeGenericType(typeof(V));
            var result = Activator.CreateInstance(type, args);

            if (result == null)
                throw new InvalidOperationException();

            return (R)result;
        }

        public abstract IAsyncSerializer<V> GetValueSerializer<V>();
        public abstract IDeserializer<V> GetValueDeserializer<V>();

        public void Dispose()
        {
            _schemaRegistry?.Dispose();
        }
    }
}
