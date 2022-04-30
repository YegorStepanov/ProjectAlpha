using UnityEngine;

namespace Code.Game;

[CreateAssetMenu(menuName = "SO/Game Settings")]
public sealed class GameSettings : ScriptableObject
{
    // [field: SerializeField]
    // public float MenuPlatformWidth { get; } = 2f;

    // [field: SerializeField]
    // public float MenuPlatformViewportPosition { get; } = 0.5f;

    // [field: SerializeField]
    // public float MenuPlatformPositionY { get; } = 0.2f;
    //
    // [field: SerializeField]
    // public float GamePlatformPositionY { get; } = 0.3f;
    //
    [field: SerializeField]
    public float MenuToGameCameraAnimationDuration { get; } = 1f;
}