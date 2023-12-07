using DiffApplication.Domain.Actions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiffApplication.Rest.ViewModels
{
    /// <summary>
    /// View model for PUT operation. 
    /// </summary>
    public class DiffViewModelPut : IValidatableObject
    {
        /// <summary>
        /// Base64 data. 
        /// </summary>
        [JsonPropertyName("data")]
        public required string Data { get; set; }

        /// <summary>
        /// Determines whether the specified data is valid. 
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var (valid, _) = IDiffResultCalculator.ConvertFromBase64String(Data);
            if (!valid)
            {
                yield return new ValidationResult("Invalid data - must be base64.");
            }
        }
    }
}
