using System.ComponentModel;
using UnityEngine;

namespace Code;

public static class BordersExtensions
{
    public static Vector3 GetRelativePoint(this Borders borders, Vector3 point, Relative relative) =>
        borders.GetRelativePoint((Vector2)point, relative).WithZ(point.z);

    public static float GetRelativePointX(this Borders borders, float pointX, Relative relative) =>
        borders.GetRelativePoint(new Vector2(pointX, 0), relative).x;

    public static float GetRelativePointY(this Borders borders, float pointY, Relative relative) =>
        borders.GetRelativePoint(new Vector2(0, pointY), relative).y;

    public static Vector2 GetRelativePoint(this Borders borders, Vector2 point, Relative relative) =>
        relative switch
        {
            Relative.Center => point - borders.Center,
            Relative.Top => point.ShiftY(-borders.Top),
            Relative.Bot => point.ShiftY(-borders.Bot),
            Relative.Left => point.ShiftX(-borders.Left),
            Relative.Right => point.ShiftX(-borders.Right),
            Relative.LeftTop => point - borders.LeftTop,
            Relative.RightTop => point - borders.RightTop,
            Relative.LeftBot => point - borders.LeftBot,
            Relative.RightBot => point - borders.RightBot,
            _ => throw new InvalidEnumArgumentException(nameof(relative), (int)relative, typeof(Relative))
        };

    public static bool Intersect(this Borders borders, Borders other)
    {
        return !(other.Left > borders.Right || other.Right < borders.Left ||
                 other.Top < borders.Bot || other.Bot > borders.Top);
    }

    public static bool Inside(this Borders borders, Borders other)
    {
        return other.Left <= borders.Left && other.Right >= borders.Right &&
               other.Top >= borders.Top && other.Bot <= borders.Bot;
    }

    public static bool Inside(this Borders borders, Vector2 point)
    {
        return point.x >= borders.Left && point.x <= borders.Right &&
               point.y >= borders.Bot && point.y <= borders.Top;
    }

    public static Borders ShiftX(this Borders borders, float offsetX) =>
        borders.Shift(new Vector2(offsetX, 0));

    public static Borders ShiftY(this Borders borders, float offsetY) =>
        borders.Shift(new Vector2(0, offsetY));

    public static Borders Shift(this Borders borders, Vector2 offset) =>
        new(borders.Top + offset.y,
            borders.Bot + offset.y,
            borders.Left + offset.x,
            borders.Right + offset.x);
}