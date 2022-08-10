using Code.Services;
using Code.Services.Entities;
using Code.Services.Monetization;
using UnityEngine;
using VContainer;

namespace Code;

[CreateAssetMenu(menuName = "Data/Settings Facade")]
public sealed class SettingsFacade : ScriptableObject
{
    [SerializeField] private BaseCamera.Settings _camera;
    [SerializeField] private Hero.Settings _hero;
    [SerializeField] private Platform.Settings _platform;
    [SerializeField] private Cherry.Settings _cherry;
    [SerializeField] private Stick.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;

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
        builder.RegisterInstance(_stick);
        builder.RegisterInstance(_stickSpawner);

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
