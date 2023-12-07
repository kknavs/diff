using DiffApplication.Domain.Models;

namespace DiffApplication.Infrastructure.Repositories
{
    public interface IDiffRepository
    {
        Diff? GetDiff(int id, Const.DiffType type);

        void PutDiff(int id, Diff diff, Const.DiffType type);
    }
}
