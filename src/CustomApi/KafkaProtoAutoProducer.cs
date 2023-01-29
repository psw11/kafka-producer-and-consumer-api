using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaProtoAutoProducer : KafkaProducer<string, KafkaProtoMessage>
    {
        public KafkaProtoAutoProducer(IKafkaProducerClient<KafkaAutoCommitConfig> kafkaClient)
            : base(kafkaClient)
        {
        }
    }
}
