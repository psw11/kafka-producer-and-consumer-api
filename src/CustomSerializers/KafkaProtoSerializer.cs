using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Kafka.BaseSerializers;
using Kafka.CustomConfig;

namespace Kafka.CustomSerializers
{
    public class KafkaProtoSerializer : KafkaSerializer
    {
        public KafkaProtoSerializer(KafkaAutoCommitConfig kafkaConfig) : base(kafkaConfig) 
        { 
        }

        public override IAsyncSerializer<V> GetValueSerializer<V>()
        {
            var protobufSerializerConfig = new ProtobufSerializerConfig
            {
                //BufferBytes = 100
            };

            return CreateGenericType<V, IAsyncSerializer<V>>(typeof(ProtobufSerializer<>), SchemaRegistry, protobufSerializerConfig);
        }

        public override IDeserializer<V> GetValueDeserializer<V>()
        {
            return CreateGenericType<V, IAsyncDeserializer<V>>(typeof(ProtobufDeserializer<>), null as IEnumerable<KeyValuePair<string, string>>)
                .AsSyncOverAsync();
        }
    }
}
