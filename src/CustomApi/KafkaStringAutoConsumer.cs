using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaStringAutoConsumer : KafkaConsumer<string, string>
    {
        public KafkaStringAutoConsumer(KafkaAutoCommitConfig kafkaConfig)
            : base(kafkaConfig)
        {
        }
    }
}
