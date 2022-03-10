using Code;
using NUnit.Framework;

namespace Tests
{
    public class MyScriptTest
    {
        [Test]
        public void MyTest()
        {
            Assert.AreEqual(3, MyScript.Field);
        }
    }
}
