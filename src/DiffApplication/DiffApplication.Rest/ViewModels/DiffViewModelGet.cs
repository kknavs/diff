using System.Text.Json.Serialization;

namespace DiffApplication.Rest.ViewModels
{
    /// <summary>
    /// View model returned if PUT succeeds. 
    /// </summary>
    public class DiffViewModelGet : DiffViewModelPut
    {
        /// <summary>
        /// Base64 data. 
        /// </summary>
        [JsonPropertyName("id")]
        public required int id { get; set; }
    }
}
