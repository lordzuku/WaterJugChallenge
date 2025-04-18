using System.Text.Json.Serialization;

namespace WaterJugChallenge.Models
{
    public class WaterJugResponse
    {
        public List<Step> Solution { get; set; } = new List<Step>();
    }

    public class Step
    {
        public int StepNumber { get; set; }
        public int BucketX { get; set; }
        public int BucketY { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Status { get; set; }
    }
} 