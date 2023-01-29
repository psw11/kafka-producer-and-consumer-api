using Newtonsoft.Json;

namespace Kafka.Messages
{
    public class KafkaJsonMessage
    {
        [JsonRequired]
        [JsonProperty("typeName")]
        public string TypeName { get; set; } = null!;

        [JsonRequired]
        [JsonProperty("typeVersion")]
        public string TypeVersion { get; set; } = null!;

        [JsonProperty("data")]
        public object? Data { get; set; }

        public override string ToString()
        {
            return $"({TypeName} {TypeVersion}, {Data})";
        }
    }
}
