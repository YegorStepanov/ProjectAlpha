using Code.Common;
using Code.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace Code.UnitTests;

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
        Borders border = new(0, 0, left, right);
        Vector2 point = new(value, 0);

        Vector2 result = border.GetRelativePoint(point, relative);

        Assert.That(result.x, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(20, 10, Relative.Center, 10, -5)]
    [TestCase(20, 10, Relative.Center, 15, 0)]
    [TestCase(20, 10, Relative.Center, 20, 5)]
    [TestCase(20, 10, Relative.Bot, 10, 0)]
    [TestCase(20, 10, Relative.Bot, 15, 5)]
    [TestCase(20, 10, Relative.Bot, 20, 10)]
    [TestCase(20, 10, Relative.Top, 10, -10)]
    [TestCase(20, 10, Relative.Top, 15, -5)]
    [TestCase(20, 10, Relative.Top, 20, 0)]
    public void GetRelativePoint_VerticalTest(float top, float bottom, Relative relative, float value, float expected)
    {
        Borders border = new(top, bottom, 0, 0);
        Vector2 point = new(0, value);

        Vector2 result = border.GetRelativePoint(point, relative);

        Assert.That(result.y, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 5, 20, 30, Relative.LeftTop, 20, 10, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.LeftTop, 30, 5, 10, -5)]
    [TestCase(10, 5, 20, 30, Relative.LeftBot, 20, 10, 0, 5)]
    [TestCase(10, 5, 20, 30, Relative.LeftBot, 20, 5, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.RightTop, 30, 10, 0, 0)]
    [TestCase(10, 5, 20, 30, Relative.RightTop, 20, 5, -10, -5)]
    [TestCase(10, 5, 20, 30, Relative.RightBot, 30, 10, 0, 5)]
    [TestCase(10, 5, 20, 30, Relative.RightBot, 30, 5, 0, 0)]
    public void GetRelativePoint_CornerTest(float top, float bottom, float left, float right, Relative relative,
        float valueX, float valueY, float expectedX, float expectedY)
    {
        Borders border = new(top, bottom, left, right);
        Vector2 point = new(valueX, valueY);

        Vector2 result = border.GetRelativePoint(point, relative);

        Assert.That(result, Is.EqualTo(new Vector2(expectedX, expectedY)));
    }

    [Test]
    public void GetRelativePoint_Vector3_PreserveZTest()
    {
        Vector3 point = new(1, 1, 30);
        Borders border = new(1, 1, 1, 1);

        Vector3 result = border.GetRelativePoint(point, Relative.Center);

        Assert.That(result.z, Is.EqualTo(30));
    }

    [Test]
    [TestCase(1, -1, -1, 1)]
    [TestCase(0, 0, 0, 0)]
    public void Inside_Reflexivity_Test(float top, float bottom, float left, float right)
    {
        Borders borders = new(top, bottom, left, right);

        Assert.That(borders.Inside(borders), Is.True);
    }


    [Test]
    [TestCase(1, -1, -1, 1)]
    public void Intersect_CornerTests(float top, float bottom, float left, float right)
    {
        Borders borders = new(top, bottom, left, right);

        Borders topPoint = new(top, 0, 0, 0);
        Borders bottomPoint = new(0, bottom, 0, 0);
        Borders leftPoint = new(0, 0, left, 0);
        Borders rightPoint = new(0, 0, 0, right);

        Assert.That(borders.Intersect(topPoint), Is.True);
        Assert.That(borders.Intersect(bottomPoint), Is.True);
        Assert.That(borders.Intersect(leftPoint), Is.True);
        Assert.That(borders.Intersect(rightPoint), Is.True);

        Assert.That(topPoint.Intersect(borders), Is.True);
        Assert.That(bottomPoint.Intersect(borders), Is.True);
        Assert.That(leftPoint.Intersect(borders), Is.True);
        Assert.That(rightPoint.Intersect(borders), Is.True);
    }

    [Test]
    [TestCase(1, -1, -1, 1)]
    public void Inside_CornerTests(float top, float bottom, float left, float right)
    {
        Borders borders = new(top, bottom, left, right);

        Borders topPoint = new(top, 0, 0, 0);
        Borders bottomPoint = new(0, bottom, 0, 0);
        Borders leftPoint = new(0, 0, left, 0);
        Borders rightPoint = new(0, 0, 0, right);

        Assert.That(topPoint.Inside(borders), Is.True);
        Assert.That(bottomPoint.Inside(borders), Is.True);
        Assert.That(leftPoint.Inside(borders), Is.True);
        Assert.That(rightPoint.Inside(borders), Is.True);
    }
}
