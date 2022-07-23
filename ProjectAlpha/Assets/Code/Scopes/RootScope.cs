using Code.AddressableAssets;
using Code.Game;
using Code.Infrastructure;
using Code.Services;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class RootScope : Scope
{
    private Camera _camera;
    private SettingsFacade _settingsFacade;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadCamera = loader.InstantiateAsync(RootAddress.CameraController, inject: false);
        var loadGameSettings = loader.LoadAssetAsync(RootAddress.Settings);
        var loadEventSystem = loader.InstantiateAsync(RootAddress.EventSystem, inject: false);

        (_camera, _settingsFacade, _) = await (loadCamera, loadGameSettings, loadEventSystem);
        LoadDevelopmentAssets(loader, _settingsFacade);
    }

    private static void LoadDevelopmentAssets(IAddressablesLoader loader, SettingsFacade settings)
    {
        if (PlatformInfo.IsDevelopment && settings.Development.GraphyInDebug)
            loader.InstantiateAsync(DebugAddress.Graphy, inject: false).Forget();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterSettings(builder);
        RegisterCancellationToken(builder);
        RegisterCamera(builder);

        builder.Register<IGameEvents, GameEvents>(Lifetime.Singleton);
        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        builder.Register<PlayerProgress>(Lifetime.Singleton);
        builder.Register<GameProgress>(Lifetime.Singleton);

        builder.RegisterBuildCallback(resolver =>
        {
            _ = resolver.Resolve<AdsManager>();
        });
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _settingsFacade.RegisterSettings(builder);
    }

    private static void RegisterCancellationToken(IContainerBuilder builder)
    {
        builder.Register<ScopeToken>(Lifetime.Scoped);
    }

    private void RegisterCamera(IContainerBuilder builder)
    {
        builder.RegisterComponent(_camera);
    }

    private static void RegisterSceneLoader(IContainerBuilder builder)
    {
        builder.RegisterInstance<ISceneLoader>(SceneLoader.Instance);
    }

    private static void RegisterLoaders(IContainerBuilder builder)
    {
        builder.Register<ICreator, Creator>(Lifetime.Scoped);
        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);
    }

    private static void RegisterAds(IContainerBuilder builder)
    {
        builder.Register<BannerAd>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();

        builder.Register<InterstitialAd>(Lifetime.Transient);
        builder.Register<RewardedAd>(Lifetime.Transient);

        builder.Register<AdInitializer>(Lifetime.Transient);
        builder.Register<Ads>(Lifetime.Singleton);

        builder.Register<AdsManager>(Lifetime.Singleton);
    }

    private static void RegisterIAP(IContainerBuilder builder)
    {
        builder.Register<IIAPManager, IAPManager>(Lifetime.Singleton);
    }
}
