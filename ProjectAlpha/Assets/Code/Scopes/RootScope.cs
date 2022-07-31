using Code.AddressableAssets;
using Code.Common;
using Code.Services.Data;
using Code.Services.Development;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer;
using VContainer.Unity;
using Progress = Code.Services.Data.Progress;

namespace Code.Scopes;

public sealed class RootScope : Scope
{
    private ICamera _camera;
    private SettingsFacade _settingsFacade;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        (_camera, _settingsFacade, _) = await (
            loader.InstantiateAsync(Address.Infrastructure.CameraController, inject: false),
            loader.LoadAssetAsync(Address.Infrastructure.Settings),
            loader.InstantiateAsync(Address.Infrastructure.EventSystem, inject: false));

        LoadDevelopmentAssets(loader, _settingsFacade);
    }

    private static void LoadDevelopmentAssets(IAddressablesLoader loader, SettingsFacade settings)
    {
        if (PlatformInfo.IsDevelopment && settings.Development.GraphyInDebug)
            loader.InstantiateAsync(Address.Development.Graphy, inject: false).Forget();
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        RegisterSettings(builder);
        RegisterCamera(builder);

        RegisterInput(builder);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        RegisterProgress(builder);
        RegisterMessagePipe(builder);
        RegisterDevelopment(builder);

        builder.RegisterEntryPoint<RootEntryPoint>();
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _settingsFacade.RegisterSettings(builder);
    }

    private void RegisterCamera(IContainerBuilder builder)
    {
        builder.RegisterComponent(_camera);
    }

    private static void RegisterInput(IContainerBuilder builder)
    {
        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
    }

    private static void RegisterSceneLoader(IContainerBuilder builder)
    {
        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
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

    private static void RegisterDevelopment(IContainerBuilder builder)
    {
        if(PlatformInfo.IsDevelopment)
            builder.RegisterComponentOnNewGameObject<DevelopmentPanel>(Lifetime.Singleton);
    }
}
