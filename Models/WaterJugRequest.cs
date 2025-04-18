using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WaterJugChallenge.Models
{
    public class WaterJugRequest
    {
        [Required(ErrorMessage = "x_capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "x_capacity must be a positive integer")]
        [JsonPropertyName("x_capacity")]
        [JsonConverter(typeof(StrictIntConverter))]
        public int X_capacity { get; set; }

        [Required(ErrorMessage = "y_capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "y_capacity must be a positive integer")]
        [JsonPropertyName("y_capacity")]
        [JsonConverter(typeof(StrictIntConverter))]
        public int Y_capacity { get; set; }

        [Required(ErrorMessage = "z_amount_wanted is required")]
        [Range(1, int.MaxValue, ErrorMessage = "z_amount_wanted must be a positive integer")]
        [JsonPropertyName("z_amount_wanted")]
        [JsonConverter(typeof(StrictIntConverter))]
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