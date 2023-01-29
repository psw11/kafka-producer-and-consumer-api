using Kafka.BaseApi;
using Kafka.CustomConfig;

namespace Kafka.CustomApi
{
    public class KafkaProtoAutoConsumer : KafkaConsumer<string, KafkaProtoMessage>
    {
        public KafkaProtoAutoConsumer(KafkaAutoCommitConfig kafkaConfig)
            : base(kafkaConfig)
        {
        }
    }
}
