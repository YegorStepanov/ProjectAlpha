using System.ComponentModel;
using Code.Common;
using UnityEngine;

namespace Code.Extensions;

public static class VectorExtensions
{
    public static Vector3 WithX(this Vector3 v, float x) => new(x, v.y, v.z);
    public static Vector3 WithY(this Vector3 v, float y) => new(v.x, y, v.z);
    public static Vector3 WithZ(this Vector3 v, float z) => new(v.x, v.y, z);

    public static Vector3 WithXY(this Vector3 v, Vector2 xy) => new(xy.x, xy.y, v.z);
    public static Vector3 ShiftXY(this Vector3 v, Vector2 xy) => new(v.x + xy.x, v.y + xy.y, v.z);

    public static Vector2 WithX(this Vector2 v, float x) => new(x, v.y);
    public static Vector2 WithY(this Vector2 v, float y) => new(v.x, y);
    public static Vector3 WithZ(this Vector2 v, float z) => new(v.x, v.y, z);

    public static Vector3 ShiftX(this Vector3 v, float offset) => new(v.x + offset, v.y, v.z);
    public static Vector3 ShiftY(this Vector3 v, float offset) => new(v.x, v.y + offset, v.z);
    public static Vector3 ShiftZ(this Vector3 v, float offset) => new(v.x, v.y, v.z + offset);

    public static Vector2 ShiftX(this Vector2 v, float offset) => new(v.x + offset, v.y);
    public static Vector2 ShiftY(this Vector2 v, float offset) => new(v.x, v.y + offset);
    public static Vector3 ShiftZ(this Vector2 v, float offset) => new(v.x, v.y, offset);

    public static Vector2 Shift(this Vector2 point, Borders borders, Relative relative)
    {
        float halfHeight = borders.Height / 2f;
        float halfWidth = borders.Width / 2f;

        return relative switch
        {
            Relative.Center => point,
            Relative.Top => point.ShiftY(-halfHeight),
            Relative.Bot => point.ShiftY(halfHeight),
            Relative.Left => point.ShiftX(halfWidth),
            Relative.Right => point.ShiftX(-halfWidth),
            Relative.LeftTop => point.ShiftX(halfWidth).ShiftY(-halfHeight),
            Relative.RightTop => point.ShiftX(-halfWidth).ShiftY(-halfHeight),
            Relative.LeftBot => point.ShiftX(halfWidth).ShiftY(halfHeight),
            Relative.RightBot => point.ShiftX(-halfWidth).ShiftY(halfHeight),
            _ => throw new InvalidEnumArgumentException(nameof(relative), (int)relative, typeof(Relative))
        };
    }
}
