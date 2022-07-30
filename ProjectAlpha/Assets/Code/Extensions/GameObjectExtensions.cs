using UnityEngine;

namespace Code.Extensions;

public static class GameObjectExtensions
{
    public static bool IsPrefab(this GameObject gameObject) =>
        gameObject.scene == default;
}
