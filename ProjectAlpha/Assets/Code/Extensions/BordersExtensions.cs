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
            Relative.Center => point.ShiftX(-(borders.Left + borders.Right) / 2f)
                .ShiftY(-(borders.Top + borders.Bottom) / 2f),
            Relative.Top => point.ShiftY(-borders.Top),
            Relative.Bottom => point.ShiftY(-borders.Bottom),
            Relative.Left => point.ShiftX(-borders.Left),
            Relative.Right => point.ShiftX(-borders.Right),
            Relative.LeftTop => point.ShiftX(-borders.Left).ShiftY(-borders.Top),
            Relative.RightTop => point.ShiftX(-borders.Right).ShiftY(-borders.Top),
            Relative.LeftBottom => point.ShiftX(-borders.Left).ShiftY(-borders.Bottom),
            Relative.RightBottom => point.ShiftX(-borders.Right).ShiftY(-borders.Bottom),
            _ => throw new InvalidEnumArgumentException(nameof(relative), (int)relative, typeof(Relative))
        };
}