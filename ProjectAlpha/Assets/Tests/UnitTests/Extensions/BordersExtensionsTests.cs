using Code;
using NUnit.Framework;
using UnityEngine;

namespace Tests.UnitTests.Extensions;

public sealed class BordersExtensionsTests
{
    [Test]
    [TestCase(10, 20, Relative.Center, 10, -5)]
    [TestCase(10, 20, Relative.Center, 15, 0)]
    [TestCase(10, 20, Relative.Center, 20, 5)]
    [TestCase(10, 20, Relative.Left, 10, 0)]
    [TestCase(10, 20, Relative.Left, 15, 5)]
    [TestCase(10, 20, Relative.Left, 20, 10)]
    [TestCase(10, 20, Relative.Right, 10, -10)]
    [TestCase(10, 20, Relative.Right, 15, -5)]
    [TestCase(10, 20, Relative.Right, 20, 0)]
    public void GetRelativePoint_HorizontalTest(float left, float right, Relative relative, float value, float expected)
    {
        var border = new Borders(0, 0, left, right);
        var point = new Vector2(value, 0);

        var result = border.GetRelativePoint(point, relative);

        Assert.That(result.x, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(20, 10, Relative.Center, 10, -5)]
    [TestCase(20, 10, Relative.Center, 15, 0)]
    [TestCase(20, 10, Relative.Center, 20, 5)]
    [TestCase(20, 10, Relative.Bottom, 10, 0)]
    [TestCase(20, 10, Relative.Bottom, 15, 5)]
    [TestCase(20, 10, Relative.Bottom, 20, 10)]
    [TestCase(20, 10, Relative.Top, 10, -10)]
    [TestCase(20, 10, Relative.Top, 15, -5)]
    [TestCase(20, 10, Relative.Top, 20, 0)]
    public void GetRelativePoint_VerticalTest(float top, float bottom, Relative relative, float value, float expected)
    {
        var border = new Borders(top, bottom, 0, 0);
        var point = new Vector2(0, value);

        var result = border.GetRelativePoint(point, relative);

        Assert.That(result.y, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 5, 20, 30, Relative.LeftTop, 20, 10, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.LeftTop, 30, 5, 10, -5)]
    [TestCase(10, 5, 20, 30, Relative.LeftBottom, 20, 10, 0, 5)]
    [TestCase(10, 5, 20, 30, Relative.LeftBottom, 20, 5, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.RightTop, 30, 10, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.RightTop, 20, 5, -10, -5)]
    [TestCase(10, 5, 20, 30, Relative.RightBottom, 30, 10, 0, 5)]
    [TestCase(10, 5, 20, 30, Relative.RightBottom, 30, 5, 0, 0)]
    public void GetRelativePoint_CornerTest(float top, float bottom, float left, float right, Relative relative, 
        float valueX, float valueY, float expectedX, float expectedY)
    {
        var border = new Borders(top, bottom, left, right);
        var point = new Vector2(valueX, valueY);

        var result = border.GetRelativePoint(point, relative);

        Assert.That(result, Is.EqualTo(new Vector2(expectedX, expectedY)));
    }

    [Test]
    public void GetRelativePoint_Vector3_PreserveZTest()
    {
        var point = new Vector3(1, 1, 30);
        var border = new Borders(1, 1, 1, 1);

        var result = border.GetRelativePoint(point, Relative.Center);

        Assert.That(result.z, Is.EqualTo(30));
    }
}