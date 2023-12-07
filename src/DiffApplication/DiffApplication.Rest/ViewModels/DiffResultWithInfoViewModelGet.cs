using System.Text.Json.Serialization;

namespace DiffApplication.Rest.ViewModels
{
    /// <summary>
    /// ViewModel that provides diffResultType 'Equals' with additional information. 
    /// </summary>
    public class DiffResultWithInfoViewModelGet : DiffResultViewModelGet
    {
        /// <summary>
        /// Array of diffs. 
        /// </summary>
        [JsonPropertyName("diffs")]
        public DiffResultInfoViewModelGet[] DiffResultInfos { get; set; }
    }
}
