using System.Text.Json.Serialization;

namespace WaterJugChallenge.Models
{
    public class WaterJugResponse
    {
        [JsonPropertyName("solution")]
        public List<Step> Solution { get; set; } = new List<Step>();
    }

    public class Step
    {
        [JsonPropertyName("step")]
        public int StepNumber { get; set; }

        [JsonPropertyName("bucketX")]
        public int BucketX { get; set; }

        [JsonPropertyName("bucketY")]
        public int BucketY { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
} 