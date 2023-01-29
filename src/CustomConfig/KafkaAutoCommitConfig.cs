using Kafka.BaseConfig;
using Microsoft.Extensions.Configuration;

namespace Kafka.CustomConfig
{
    public class KafkaAutoCommitConfig : KafkaConfig, IKafkaConfig
    {
        protected override string KafkaRootSectionName => "KafkaAutoCommit";

        public KafkaAutoCommitConfig(IConfiguration config)
            : base(config) 
        { 
        }
    }
}
