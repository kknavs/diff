using DiffApplication.Domain.Models;

namespace DiffApplication.Infrastructure.Repositories
{
    public interface IDiffRepository
    {
        Task<Diff?> GetDiffAsync(int id, Const.DiffType type);

        Task PutDiffAsync(int id, Diff diff, Const.DiffType type);
    }
}
