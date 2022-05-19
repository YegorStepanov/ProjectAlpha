using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace Code;

public static class BordersExtensions
{
    public static float GetRelativePointX(this Borders borders, float pointX, Relative relative) =>
        borders.GetRelativePoint(new Vector2(pointX, 0), relative).x;

    public static float GetRelativePointY(this Borders borders, float pointY, Relative relative) =>
        borders.GetRelativePoint(new Vector2(0, pointY), relative).y;

    public static Vector2 GetRelativePoint(this Borders borders, Vector2 point, Relative relative) =>
        borders.GetRelativePoint((Vector3)point, relative);

    [PublicAPI]
    public static Vector3 GetRelativePoint(this Borders borders, Vector3 point, Relative relative)
    {
        float halfHeight = borders.Height / 2f;
        float halfWidth = borders.Width / 2f;

        return relative switch
        {
            Relative.Center => point,
            Relative.Top => point.ShiftY(-halfHeight),
            Relative.Bottom => point.ShiftY(halfHeight),
            Relative.Left => point.ShiftX(halfWidth),
            Relative.Right => point.ShiftX(-halfWidth),
            Relative.LeftTop => point.ShiftX(halfWidth).ShiftY(-halfHeight),
            Relative.RightTop => point.ShiftX(-halfWidth).ShiftY(-halfHeight),
            Relative.LeftBottom => point.ShiftX(halfWidth).ShiftY(halfHeight),
            Relative.RightBottom => point.ShiftX(-halfWidth).ShiftY(halfHeight),
            _ => throw new InvalidEnumArgumentException(nameof(relative), (int)relative, typeof(Relative))
        };
    }
}