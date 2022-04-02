using System.Collections;
using Code.Game;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    public class MyScriptTest
    {
        [Test]
        public void MyTest()
        {
            _ = Substitute.For<IEnumerable>();
            MyScript.Field.Should().Be(3);
        }

        [Test]
        public void MyTest1() =>
            MyScript.UnitTaskName.Should().Be("UniTask");

        [Test]
        public void MyTest2() =>
            MyScript.UnitTaskName.Should().Be("UniTask1");
    }
}