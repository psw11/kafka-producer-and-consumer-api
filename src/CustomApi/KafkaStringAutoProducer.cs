using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaStringAutoProducer : KafkaProducer<string, string>
    {
        public KafkaStringAutoProducer(IKafkaProducerClient<KafkaAutoCommitConfig> kafkaClient)
            : base(kafkaClient)
        {
        }
    }
}
