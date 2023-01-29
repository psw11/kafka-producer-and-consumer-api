using Confluent.Kafka;

namespace Kafka.BaseSerializers
{
    public interface IKafkaSerializer : IDisposable
    {
        IAsyncSerializer<V> GetValueSerializer<V>();
        IDeserializer<V> GetValueDeserializer<V>();
    }
}