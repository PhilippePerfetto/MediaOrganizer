using SeconProjet;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Calc c = new Calc();
            var result = c.Add(6, 3);
            Assert.AreEqual(result, 9);
        }
    }
}