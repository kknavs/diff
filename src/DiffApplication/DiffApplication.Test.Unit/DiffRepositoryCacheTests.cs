using DiffApplication.Domain.Models;
using DiffApplication.Infrastructure.Repositories;

namespace DiffApplication.Test.Unit
{
    [TestClass]
    public class DiffRepositoryCacheTests
    {
        private readonly DiffRepositoryCache _repo;

        public DiffRepositoryCacheTests()
        {
            _repo = new DiffRepositoryCache();
        }

        [TestMethod]
        public void TestMultipleSameDiffPutArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "AAA=" };

            _repo.PutDiff(1, diff1, Const.DiffType.Right);
            _repo.PutDiff(1, diff1, Const.DiffType.Left);
            _repo.PutDiff(1, diff1, Const.DiffType.Right);
        }

        // could add additional tests ...
    }
}
