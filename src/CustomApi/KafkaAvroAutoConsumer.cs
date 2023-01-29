using Kafka.BaseApi;
using Kafka.CustomConfig;
using Kafka.Messages;

namespace Kafka.CustomApi
{
    public class KafkaAvroAutoConsumer : KafkaConsumer<string, KafkaAvroMessage>
    {
        public KafkaAvroAutoConsumer(KafkaAutoCommitConfig kafkaConfig)
            : base(kafkaConfig)
        {
        }
    }
}
