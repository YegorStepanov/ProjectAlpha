using System.ComponentModel;
using UnityEngine;

namespace Code;

public static class BordersExtensions
{
    public static Vector3 GetRelativePoint(this Borders borders, Vector3 point, Relative relative) =>
        borders.GetRelativePoint((Vector2)point, relative).WithZ(point.z);

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
}