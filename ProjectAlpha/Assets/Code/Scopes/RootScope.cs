using Code.AddressableAssets;
using Code.Game;
using Code.Infrastructure;
using Code.Services;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer;
using VContainer.Unity;
using Progress = Code.Services.Progress;

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
        builder.Register<ScopeCancellationToken>(Lifetime.Scoped);

        RegisterSettings(builder);
        RegisterCamera(builder);

        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        RegisterProgress(builder);
        RegisterMessagePipe(builder);

        builder.RegisterEntryPoint<RootEntryPoint>();
    }

    private static void RegisterProgress(IContainerBuilder builder)
    {
        builder.Register<IPersistentProgress, PersistentProgress>(Lifetime.Singleton);
        builder.Register<ISessionProgress, SessionProgress>(Lifetime.Singleton);
        builder.Register<IProgress, Progress>(Lifetime.Singleton);
    }

    private static void RegisterMessagePipe(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();
        // Setup GlobalMessagePipe to enable diagnostics window and global function
        builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
        builder.RegisterMessageBroker<Event.GameStart>(options);
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _settingsFacade.RegisterSettings(builder);
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
        builder.Register<IAds, Ads>(Lifetime.Singleton);

        builder.Register<AdsManager>(Lifetime.Singleton);
    }

    private static void RegisterIAP(IContainerBuilder builder)
    {
        builder.Register<IIAPManager, IAPManager>(Lifetime.Singleton);
    }
}
