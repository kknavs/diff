using DiffApplication.Domain.Models;

namespace DiffApplication.Domain.Actions
{
    public class DiffResultCalculator : IDiffResultCalculator
    {
        public DiffResult GetDiffResult(Diff diff1, Diff diff2)
        {
            if (diff1 == null || diff2 == null)
            {
                throw new ArgumentException("Compared diffs must be non nullable.");
            }
            if (diff1.Id != diff2.Id)
            {
                throw new ArgumentException("Compared diffs must have same id.");
            }
            var (valid1, bytes1) = IDiffResultCalculator.ConvertFromBase64String(diff1.Data);
            var (valid2, bytes2) = IDiffResultCalculator.ConvertFromBase64String(diff2.Data);
            if (!valid1 || !valid2)
            {
                throw new ArgumentException("Invalid data - not base64.");
            }
            if (bytes1.Length != bytes2.Length)
            {
                return new DiffResult(Const.DiffResultType.SizeDoNotMatch);
            }
            List<DiffResultInfo> infos = [];
            bool diffFound = false;
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i])
                {
                    if (!diffFound)
                    {
                        infos.Add(new DiffResultInfo() { Offset = i, Length = 1 });
                    }
                    else
                    {
                        infos.Last().Length++;
                    }
                    diffFound = true;
                }
                else
                {
                    diffFound = false;
                }
            }
            if (infos.Count > 0)
            {
                return new DiffResult() 
                { 
                    DiffResultType = Const.DiffResultType.ContentDoNotMatch, 
                    DiffResultInfos = infos
                };
            }

            return new DiffResult(Const.DiffResultType.DiffEquals);
        }
    }
}
