using Kafka.BaseApi;
using Kafka.CustomConfig;
using Kafka.Messages;

namespace Kafka.CustomApi
{
    public class KafkaJsonAutoProducer : KafkaProducer<string, KafkaJsonMessage>
    {
        public KafkaJsonAutoProducer(IKafkaProducerClient<KafkaAutoCommitConfig> kafkaClient)
            : base(kafkaClient)
        {
        }
    }
}
