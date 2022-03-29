using UnityEngine;

namespace Code
{
    public static class SpriteHelper
    {
        public static Vector2 WorldSpriteSize(Sprite sprite, Vector2 worldScale)
        {
            Vector2 localSize = sprite.rect.size / sprite.pixelsPerUnit;
            return localSize * worldScale;
        }
    }
}