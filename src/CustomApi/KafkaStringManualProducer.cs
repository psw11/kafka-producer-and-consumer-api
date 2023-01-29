using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaStringManualProducer : KafkaProducer<string, string>
    {
        public KafkaStringManualProducer(IKafkaProducerClient<KafkaManualCommitConfig> kafkaClient)
            : base(kafkaClient)
        {
        }
    }
}
