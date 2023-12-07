using System.Text.Json.Serialization;

namespace DiffApplication.Rest.ViewModels
{
    /// <summary>
    /// ViewModel that provides info where the diff are. 
    /// </summary>
    public class DiffResultInfoViewModelGet
    {
        /// <summary>
        /// Offset of difference. 
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Length.
        /// </summary>
        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
