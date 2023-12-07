using DiffApplication.Domain.Actions;
using DiffApplication.Domain.Models;

namespace DiffApplication.Test.Unit
{
    [TestClass]
    public class DiffResultCalculatorTests
    {
        private readonly IDiffResultCalculator _calculator;

        public DiffResultCalculatorTests()
        {
            _calculator = new DiffResultCalculator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Compared diffs must be non nullable.")]
        public void TestInvalidNullArgument()
        {
            _calculator.GetDiffResult(null, new Diff() { Data = "data" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Compared diffs must have same id.")]
        public void TestInvalidIdArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "data" };
            var diff2 = new Diff() { Id = 2, Data = "data" };
            _calculator.GetDiffResult(diff1, diff2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid data - not base64.")]
        public void TestInvalidDataArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "dataw" };
            var diff2 = new Diff() { Id = 1, Data = "dataw" };
            _calculator.GetDiffResult(diff1, diff2);
        }

        [TestMethod]
        public void TestDiffEqualArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "AAA=" };
            var diff2 = new Diff() { Id = 1, Data = "AAA=" };
            var result = _calculator.GetDiffResult(diff1, diff2);
            Assert.AreEqual(Const.DiffResultType.DiffEquals, result.DiffResultType);
        }

        [TestMethod]
        public void TestSizeDoNotMatchArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "AAA=" };
            var diff2 = new Diff() { Id = 1, Data = "AAAAAA==" };
            var result = _calculator.GetDiffResult(diff1, diff2);
            Assert.AreEqual(Const.DiffResultType.SizeDoNotMatch, result.DiffResultType);
        }

        [TestMethod]
        public void TestContentDoNotMatchArguments()
        {
            var diff1 = new Diff() { Id = 1, Data = "AAAAAA==" };
            var diff2 = new Diff() { Id = 1, Data = "AQABAQ==" };
            var result = _calculator.GetDiffResult(diff1, diff2);
            Assert.AreEqual(Const.DiffResultType.ContentDoNotMatch, result.DiffResultType);
            Assert.IsNotNull(result.DiffResultInfos);
            Assert.AreEqual(2, result.DiffResultInfos.Count);
            Assert.AreEqual(0, result.DiffResultInfos[0].Offset);
            Assert.AreEqual(1, result.DiffResultInfos[0].Length);
            Assert.AreEqual(2, result.DiffResultInfos[1].Offset);
            Assert.AreEqual(2, result.DiffResultInfos[1].Length);
        }
    }
}