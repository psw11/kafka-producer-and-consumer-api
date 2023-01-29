using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Kafka.BaseSerializers;
using Kafka.CustomConfig;

namespace Kafka.CustomSerializers
{
    public class KafkaAvroSerializer : KafkaSerializer
    {
        public KafkaAvroSerializer(KafkaAutoCommitConfig kafkaConfig) : base(kafkaConfig) 
        { 
        }

        public override IAsyncSerializer<V> GetValueSerializer<V>()
        {
            var avroSerializerConfig = new AvroSerializerConfig
            {
                BufferBytes = 100
            };

            return new AvroSerializer<V>(SchemaRegistry, avroSerializerConfig);
        }

        public override IDeserializer<V> GetValueDeserializer<V>()
        {
            return new AvroDeserializer<V>(SchemaRegistry).AsSyncOverAsync();
        }
    }
}
