using Code.Services;
using Code.Services.Game.UI;
using UnityEngine;
using VContainer;

namespace Code.Game;

[CreateAssetMenu(menuName = "Data/Game Settings")]
public sealed class GameSettings : ScriptableObject
{
    [SerializeField] private Hero.Settings _hero;
    [SerializeField] private Platform.Settings _platform;
    [SerializeField] private Stick.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    [SerializeField] private PlatformSpawner.Settings _platformSpawner;
    [SerializeField] private GameData.Settings _gameData;

    public void RegisterAllSettings(IContainerBuilder builder)
    {
        builder.RegisterInstance(_hero);
        builder.RegisterInstance(_platform);
        builder.RegisterInstance(_stick);
        builder.RegisterInstance(_stickSpawner);
        builder.RegisterInstance(_platformSpawner);
        builder.RegisterInstance(_gameData);
    }
}