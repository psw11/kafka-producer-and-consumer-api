using Kafka.BaseApi;
using Kafka.CustomConfig;
using Kafka.Messages;

namespace Kafka.CustomApi
{
    public class KafkaJsonAutoConsumer : KafkaConsumer<string, KafkaJsonMessage>
    {
        public KafkaJsonAutoConsumer(KafkaAutoCommitConfig kafkaConfig)
            : base(kafkaConfig)
        {
        }
    }
}
