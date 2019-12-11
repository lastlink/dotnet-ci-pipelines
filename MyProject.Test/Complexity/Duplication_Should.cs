using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.Complexity;

namespace MyProject.Test.Complexity
{
    [TestClass]
    public class Duplication_Should
    {
        [TestMethod]
        public void VerifyMultiplyByTwo()
        {
            const int number = 2;

            Assert.AreEqual(Duplication.multiplyByTwo(number), 4);
        }
    }
}