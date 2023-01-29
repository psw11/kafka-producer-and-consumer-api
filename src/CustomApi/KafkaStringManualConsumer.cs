using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaStringManualConsumer : KafkaConsumer<string, string>
    {
        public KafkaStringManualConsumer(KafkaAutoCommitConfig kafkaConfig)
            : base(kafkaConfig)
        {
        }
    }
}
