using Code.Common;
using UnityEngine;

namespace Code.Extensions
{
    public static class BoundsExtensions
    {
        public static Borders AsBorders(this Bounds bounds) =>
            new(bounds.max.y, bounds.min.y, bounds.min.x, bounds.max.x);
    }
}