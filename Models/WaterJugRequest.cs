using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WaterJugChallenge.Models
{
    public class WaterJugRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "X capacity must be a positive integer")]
        public int X_capacity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Y capacity must be a positive integer")]
        public int Y_capacity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Target amount must be a positive integer")]
        public int Z_amount_wanted { get; set; }
    }

    public class StrictIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int value))
                {
                    return value;
                }
                throw new JsonException("the provided value is not a positive integer");
            }
            throw new JsonException("the provided value is not a positive integer");
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
} 