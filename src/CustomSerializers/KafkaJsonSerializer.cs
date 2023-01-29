using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Kafka.BaseSerializers;
using Kafka.CustomConfig;

namespace Kafka.CustomSerializers
{
    public class KafkaJsonSerializer : KafkaSerializer
    {
        public KafkaJsonSerializer(KafkaAutoCommitConfig kafkaConfig) : base(kafkaConfig) 
        { 
        }

        public override IAsyncSerializer<V> GetValueSerializer<V>()
        {
            var jsonSerializerConfig = new JsonSerializerConfig
            {
                //BufferBytes = 100
            };

            return CreateGenericType<V, IAsyncSerializer<V>>(typeof(JsonSerializer<>), SchemaRegistry, jsonSerializerConfig, null);
        }

        public override IDeserializer<V> GetValueDeserializer<V>() 
        {
            return CreateGenericType<V, IAsyncDeserializer<V>>(typeof(JsonDeserializer<>), null, null)
                .AsSyncOverAsync();
        }
    }
}
