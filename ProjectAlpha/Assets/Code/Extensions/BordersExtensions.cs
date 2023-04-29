using Code.Common;
using UnityEngine;

namespace Code.Extensions
{
    public static class BordersExtensions
    {
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

        public static Borders Shift(this Borders borders, Vector2 offset) =>
            new(borders.Top + offset.y,
                borders.Bot + offset.y,
                borders.Left + offset.x,
                borders.Right + offset.x);
    }
}