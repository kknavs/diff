using DiffApplication.Domain.Models;
using System.Collections.Concurrent;

namespace DiffApplication.Infrastructure.Repositories
{
    public class DiffRepositoryCache : IDiffRepository
    {
        static readonly ConcurrentDictionary<int, Diff> _leftDiffs = new();
        static readonly ConcurrentDictionary<int, Diff> _rightDiffs = new();

        public DiffRepositoryCache() 
        { 
        }

        ConcurrentDictionary<int, Diff> GetSelectedDict(Const.DiffType type)
        {
            if (type == Const.DiffType.Right)
            {
                return _rightDiffs;
            }
            return _leftDiffs;
        }

        public Diff? GetDiff(int id, Const.DiffType type)
        {
            var selectedDict = GetSelectedDict(type);
            return selectedDict.GetValueOrDefault(id);
        }

        public void PutDiff(int id, Diff diff, Const.DiffType type)
        {
            var selectedDict = GetSelectedDict(type);
            selectedDict[id] = diff;
        }
    }
}
