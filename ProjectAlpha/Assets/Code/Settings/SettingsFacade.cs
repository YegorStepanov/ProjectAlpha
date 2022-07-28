using Code.Infrastructure;
using Code.Services;
using Code.Services.Game.UI;
using Code.Services.Monetization;
using Code.States;
using UnityEngine;
using VContainer;
using Camera = Code.Services.Camera;

namespace Code.Game;

[CreateAssetMenu(menuName = "Data/Game Settings")]
public sealed class SettingsFacade : ScriptableObject
{
    [SerializeField] private Camera.Settings _camera;
    [SerializeField] private Hero.Settings _hero;
    [SerializeField] private Platform.Settings _platform;
    [SerializeField] private Cherry.Settings _cherry;
    [SerializeField] private CherrySpawner.Settings _cherrySpawner;
    [SerializeField] private Stick.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    [SerializeField] private PlatformSpawner.Settings _platformSpawner;
    [SerializeField] private GameWorld.Settings _gameWorld;
    [SerializeField] private GameLoopSettings _gameLoopSettings;
    [SerializeField] private AdsManager.Settings _adsManager;
    [SerializeField] private IAPManager.Settings _iapManager;
    [SerializeField] private Ads.Settings _androidAdsProvider;
    [SerializeField] private Ads.Settings _iosAdsProvider;
    [SerializeField] private DevelopmentSettings _developmentSettings;

    public DevelopmentSettings Development => _developmentSettings;

    public void RegisterSettings(IContainerBuilder builder)
    {
        builder.RegisterInstance(_camera);
        builder.RegisterInstance(_hero);
        builder.RegisterInstance(_platform);
        builder.RegisterInstance(_cherry);
        builder.RegisterInstance(_cherrySpawner);
        builder.RegisterInstance(_stick);
        builder.RegisterInstance(_stickSpawner);
        builder.RegisterInstance(_platformSpawner);
        builder.RegisterInstance(_gameWorld);
        builder.RegisterInstance(_gameLoopSettings);
        builder.RegisterInstance(_adsManager);
        builder.RegisterInstance(_iapManager);

        if (PlatformInfo.IsApple)
            builder.RegisterInstance(_iosAdsProvider);
        else
            builder.RegisterInstance(_androidAdsProvider);
    }
}
