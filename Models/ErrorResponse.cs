namespace WaterJugChallenge.Models
{
    /// <summary>
    /// Represents an error response from the API
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The error message describing what went wrong
        /// </summary>
        public string Error { get; set; } = string.Empty;
    }
} 