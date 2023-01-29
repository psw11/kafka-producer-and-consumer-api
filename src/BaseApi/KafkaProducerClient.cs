using Confluent.Kafka;
using Kafka.BaseConfig;

namespace Kafka.BaseApi
{
    public class KafkaProducerClient<TKafkaConfig> : IDisposable, IKafkaProducerClient<TKafkaConfig>
        where TKafkaConfig : IKafkaConfig
    {
        public IAdminClient KafkaAdmin { get; }
        public IProducer<byte[], byte[]> KafkaDefaultProducer { get; }

        public KafkaProducerClient(TKafkaConfig kafkaConfig)
        {
            var adminConfig = kafkaConfig.GetAdminConfig();
            KafkaAdmin = new AdminClientBuilder(adminConfig).Build();

            var producerConfig = kafkaConfig.GetProducerConfig();
            KafkaDefaultProducer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
        }

        public void Dispose()
        {
            KafkaDefaultProducer.Flush();       // Send all records.
            KafkaDefaultProducer.Dispose();
            KafkaAdmin.Dispose();
        }
    }
}
