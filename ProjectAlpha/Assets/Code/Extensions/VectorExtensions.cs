using UnityEngine;

namespace Code
{
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
    }
}