using System.ComponentModel;
using Code.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Extensions
{
    public static class BordersExtensions
    {
        
        public static float TransformPointX(this Borders borders, float pointX, Relative relative) =>
            borders.TransformPoint(new Vector2(pointX, 0), relative).x;

        public static float TransformPointY(this Borders borders, float pointY, Relative relative) =>
            borders.TransformPoint(new Vector2(0, pointY), relative).y;

        public static Vector2 TransformPoint(this Borders borders, Vector2 point, Relative relative) =>
            borders.TransformPoint((Vector3)point, relative);

        [PublicAPI]
        public static Vector3 TransformPoint(this Borders borders, Vector3 point, Relative relative)
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
}