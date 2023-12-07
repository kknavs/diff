using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DiffApplication.Domain.Models
{
    public class DiffResult
    {
        public DiffResult()
        {
        }

        public DiffResult(string diffResultType) 
        {
            DiffResultType = diffResultType;
        }

        [Required, NotNull]
        public string DiffResultType { get; set; }

        public List<DiffResultInfo> DiffResultInfos { get; set; } = [];
    }
}
