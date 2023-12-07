using System.Text.Json.Serialization;

namespace DiffApplication.Rest.ViewModels
{
    /// <summary>
    /// ViewModel that provides diffResultType. 
    /// </summary>
    public class DiffResultViewModelGet
    {
        /// <summary>
        /// Equals, SizeDoNotMatch or ContentDoNotMatch. 
        /// </summary>
        [JsonPropertyName("diffResultType")]
        public required string DiffResultType { get; set; }
    }
}
