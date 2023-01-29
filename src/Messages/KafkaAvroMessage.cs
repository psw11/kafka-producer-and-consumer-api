using Avro;
using Avro.Specific;

namespace Kafka.Messages
{
    public class KafkaAvroMessage : ISpecificRecord
    {
        public string TypeName { get; set; } = null!;
        public string TypeVersion { get; set; } = null!;
        public string? Data { get; set; }

        public static Schema _SCHEMA = Schema.Parse(File.ReadAllText("Messages\\AvroSchema\\avro_schema.json"));
        public Schema Schema => _SCHEMA;

        public object Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return TypeName;
                case 1: return TypeVersion;
                case 2: return Data;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
            };
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: TypeName = (string)fieldValue; break;
                case 1: TypeVersion = (string)fieldValue; break;
                case 2: Data = (string)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            };
        }

        public override string ToString()
        {
            return $"({TypeName} {TypeVersion}, {Data})";
        }
    }
}
