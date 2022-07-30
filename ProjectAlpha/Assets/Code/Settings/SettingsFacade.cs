using Code.Common;
using Code.Services;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Entities.Stick;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using Code.Services.Monetization.IAP;
using Code.Services.Spawners;
using UnityEngine;
using VContainer;

namespace Code.Settings;

[CreateAssetMenu(menuName = "Data/Settings Facade")]
public sealed class SettingsFacade : ScriptableObject
{
    [SerializeField] private Camera1.Settings _camera;
    [SerializeField] private Hero.Settings _hero;
    [SerializeField] private Platform.Settings _platform;
    [SerializeField] private Cherry.Settings _cherry;
    [SerializeField] private CherrySpawner.Settings _cherrySpawner;
    [SerializeField] private Stick.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    [SerializeField] private PlatformSpawner.Settings _platformSpawner;
    [SerializeField] private GameWorld.Settings _gameWorld;

    [SerializeField] private AdsManager.Settings _adsManager;
    [SerializeField] private IAPManager.Settings _iapManager;
    [SerializeField] private AdsSettings _androidAdsProvider;
    [SerializeField] private AdsSettings _iosAdsProvider;

    [SerializeField] private GameSettings _gameSettings;

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

        builder.RegisterInstance(_gameSettings);

        RegisterMonetization(builder);
    }

    private void RegisterMonetization(IContainerBuilder builder)
    {
        builder.RegisterInstance(_adsManager);
        builder.RegisterInstance(_iapManager);

        if (PlatformInfo.IsApple)
            builder.RegisterInstance(_iosAdsProvider);
        else
            builder.RegisterInstance(_androidAdsProvider);
    }
}
