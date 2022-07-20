using Code.Infrastructure;
using Code.Services;
using Code.Services.Game.UI;
using Code.Services.Monetization;
using UnityEngine;
using VContainer;

namespace Code.Game;

[CreateAssetMenu(menuName = "Data/Game Settings")]
public sealed class SettingsFacade : ScriptableObject
{
    [SerializeField] private Hero.Settings _hero;
    [SerializeField] private Platform.Settings _platform;
    [SerializeField] private Cherry.Settings _cherry;
    [SerializeField] private CherrySpawner.Settings _cherrySpawner;
    [SerializeField] private Stick.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    [SerializeField] private PlatformSpawner.Settings _platformSpawner;
    [SerializeField] private GameWorld.Settings _gameWorld;
    [SerializeField] private GameStateMachine.Settings _gameStateMachine;
    [SerializeField] private AdsSettings _androidAdsProvider;
    [SerializeField] private AdsSettings _iosAdsProvider;

    public void RegisterSettings(IContainerBuilder builder)
    {
        builder.RegisterInstance(_hero);
        builder.RegisterInstance(_platform);
        builder.RegisterInstance(_cherry);
        builder.RegisterInstance(_cherrySpawner);
        builder.RegisterInstance(_stick);
        builder.RegisterInstance(_stickSpawner);
        builder.RegisterInstance(_platformSpawner);
        builder.RegisterInstance(_gameWorld);
        builder.RegisterInstance(_gameStateMachine);

        if (PlatformInfo.IsApple)
            builder.RegisterInstance(_iosAdsProvider);
        else
            builder.RegisterInstance(_androidAdsProvider);
    }
}
