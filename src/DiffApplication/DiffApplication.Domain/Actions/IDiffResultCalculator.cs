using DiffApplication.Domain.Models;

namespace DiffApplication.Domain.Actions
{
    public interface IDiffResultCalculator
    {
        DiffResult GetDiffResult(Diff diff1, Diff diff2);

        public static (bool valid, byte[] buffer) ConvertFromBase64String(string base64)
        {
            byte[] bytes = new byte[base64.Length];
            if (Convert.TryFromBase64String(base64, bytes, out int _))
            {
                return (true, bytes);
            }
            return (false, bytes);
        }
    }
}
