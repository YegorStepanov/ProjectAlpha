using Code.Services;
using UnityEngine;
using VContainer;

namespace Code.Game;

[CreateAssetMenu(menuName = "Data/Game Settings")]
public sealed class GameSettings : ScriptableObject
{
    [SerializeField] private HeroController.Settings _hero;
    [SerializeField] private PlatformController.Settings _platform;
    [SerializeField] private StickController.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    [SerializeField] private PlatformSpawner.Settings _platformSpawner;

    public void RegisterAllSettings(IContainerBuilder builder)
    {
        builder.RegisterInstance(_hero);
        builder.RegisterInstance(_platform);
        builder.RegisterInstance(_stick);
        builder.RegisterInstance(_stickSpawner);
        builder.RegisterInstance(_platformSpawner);
    }
}