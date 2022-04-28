using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using Zenject;

public sealed class MyUnitTest : ZenjectUnitTestFixture
{
    [Test]
    public void Test1()
    {
        1.Should().Be(1);
    }
}