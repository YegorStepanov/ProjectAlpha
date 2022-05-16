using UnityEngine;

namespace Code;

public static class GameObjectExtensions
{
    public static bool IsPrefab(this GameObject gameObject)
    {
        return gameObject.scene == default;
    }
}