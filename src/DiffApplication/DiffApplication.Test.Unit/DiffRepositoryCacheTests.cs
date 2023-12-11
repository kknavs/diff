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
        public async Task TestUpdateSameDiffAsync()
        {
            int id = 1;
            var diff1 = new Diff() { Id = id, Data = "AAA=" };

            await _repo.PutDiffAsync(id, diff1, Const.DiffType.Right);
            var diff = await _repo.GetDiffAsync(id, Const.DiffType.Right);
            Assert.IsNotNull(diff);
            Assert.AreEqual(diff.Id, diff1.Id);
            Assert.AreEqual(diff.Data, diff1.Data);

            diff1.Data = "AAAAAA==";
            await _repo.PutDiffAsync(id, diff1, Const.DiffType.Right);
            diff = await _repo.GetDiffAsync(id, Const.DiffType.Right);
            Assert.IsNotNull(diff);
            Assert.AreEqual(diff.Id, diff1.Id);
            Assert.AreEqual(diff.Data, diff1.Data);

            diff = await _repo.GetDiffAsync(id, Const.DiffType.Left);
            Assert.IsNull(diff);
        }

        // could add additional tests ...
    }
}
