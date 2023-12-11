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

        public Task<Diff?> GetDiffAsync(int id, Const.DiffType type)
        {
            var selectedDict = GetSelectedDict(type);
            return Task.FromResult(selectedDict.GetValueOrDefault(id));
        }

        public Task PutDiffAsync(int id, Diff diff, Const.DiffType type)
        {
            var selectedDict = GetSelectedDict(type);
            selectedDict[id] = diff;
            return Task.CompletedTask;
        }
    }
}
