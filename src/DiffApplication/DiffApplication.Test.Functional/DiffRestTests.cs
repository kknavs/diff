using DiffApplication.Domain.Models;
using DiffApplication.Rest.ViewModels;
using System.Net;

namespace DiffApplication.Test.Functional
{
    [TestClass]
    public class DiffRestTests : TestBase
    {
        static string GetDiffUrl(int id)
        {
            return $"v1/diff/{id}";
        }

        static string GetDiffUrl(int id, Const.DiffType type)
        {
            return $"v1/diff/{id}/{Enum.GetName(type)!.ToLower()}";
        }

        [TestMethod]
        public async Task PutDiffLeft()
        {
            int id = 1;
            var diff = new DiffViewModelPut() { Data = "AAA=" };
            var response = await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Left), diff, HttpStatusCode.Created);
            var returnedDiff = await ParseResponse<DiffViewModelGet>(response);
            Assert.IsNotNull(returnedDiff);
            Assert.AreEqual(id, returnedDiff.id);
            Assert.AreEqual(diff.Data, returnedDiff.Data);
        }

        [TestMethod]
        public async Task PutDiffRight()
        {
            int id = 1;
            var diff = new DiffViewModelPut() { Data = "AAA=" };
            var response = await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Right), diff, HttpStatusCode.Created);
            var returnedDiff = await ParseResponse<DiffViewModelGet>(response);
            Assert.IsNotNull(returnedDiff);
            Assert.AreEqual(id, returnedDiff.id);
            Assert.AreEqual(diff.Data, returnedDiff.Data);
        }

        [TestMethod]
        public async Task PutDiffInvalid()
        {
            int id = 1;
            var diff = new DiffViewModelPut() { Data = "nfeinf" };
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Right), diff, HttpStatusCode.BadRequest);
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Left), diff, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task GetDiffNotFound()
        {
            await GetAssertAsync<DiffResultViewModelGet>(client, GetDiffUrl(123), HttpStatusCode.NotFound);

            int id = 1;
            var diff = new DiffViewModelPut() { Data = "AQABAQ==" };
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Right), diff, HttpStatusCode.Created);
            await PutAssertAsync(client, GetDiffUrl(2, Const.DiffType.Left), diff, HttpStatusCode.Created);
            await GetAssertAsync<DiffResultViewModelGet>(client, GetDiffUrl(1), HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetDiffWithInfoEquals()
        {
            int id = 1;
            var diff = new DiffViewModelPut() { Data = "AQABAQ==" };
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Right), diff, HttpStatusCode.Created);
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Left), diff, HttpStatusCode.Created);

            var result = await GetAssertAsync<DiffResultWithInfoViewModelGet>(client, GetDiffUrl(1), HttpStatusCode.OK);
            Assert.IsNotNull(result);
            Assert.AreEqual(Const.DiffResultType.DiffEquals, result.DiffResultType);
            Assert.IsNull(result.DiffResultInfos);
        }

        [TestMethod]
        public async Task GetDiffWithInfoContentDismatch()
        {
            int id = 1;
            var diff1 = new DiffViewModelPut() { Data = "AQABAQ==" };
            var diff2 = new DiffViewModelPut() { Data = "AAAAAA==" };
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Right), diff1, HttpStatusCode.Created);
            await PutAssertAsync(client, GetDiffUrl(id, Const.DiffType.Left), diff2, HttpStatusCode.Created);

            var result = await GetAssertAsync<DiffResultWithInfoViewModelGet>(client, GetDiffUrl(1), HttpStatusCode.OK);
            Assert.IsNotNull(result);
            Assert.AreEqual(Const.DiffResultType.ContentDoNotMatch, result.DiffResultType);
            Assert.IsNotNull(result.DiffResultInfos);
            Assert.AreEqual(2, result.DiffResultInfos.Length);
        }

        // could add additional tests ...
    }
}