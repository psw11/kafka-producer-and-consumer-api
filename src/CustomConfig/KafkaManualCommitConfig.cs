using Kafka.BaseConfig;
using Microsoft.Extensions.Configuration;

namespace Kafka.CustomConfig
{
    public class KafkaManualCommitConfig : KafkaConfig, IKafkaConfig
    {
        protected override string KafkaRootSectionName => "KafkaManualCommit";

        public KafkaManualCommitConfig(IConfiguration config)
            : base(config) 
        { 
        }
    }
}
