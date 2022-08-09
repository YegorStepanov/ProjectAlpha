using Code.Common;
using Code.Extensions;
using NUnit.Framework;

namespace Code.UnitTests;

public sealed class BordersExtensionsTests
{
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
