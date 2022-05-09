using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

public sealed class MyUnitTest //: ZenjectUnitTestFixture
{
    [Test]
    public void Test1()
    {
        1.Should().Be(1);
    }

    [Test]
    public void Test2()
    {
        Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
    }

    public sealed class StartGameTrigger : MonoBehaviour
    {
        private GameObject gameTriggers;
    }
}