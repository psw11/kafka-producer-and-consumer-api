using Kafka.BaseApi;
using Kafka.CustomConfig;
using Kafka.Messages;

namespace Kafka.CustomApi
{
    public class KafkaAvroAutoProducer : KafkaProducer<string, KafkaAvroMessage>
    {
        public KafkaAvroAutoProducer(IKafkaProducerClient<KafkaAutoCommitConfig> kafkaClient)
            : base(kafkaClient)
        {
        }
    }
}
