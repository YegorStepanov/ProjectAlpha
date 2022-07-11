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
    private GameSettings _gameSettings;

    // todo:
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    // public static void InitUniTaskLoop()
    // {
    //     //var loop = PlayerLoop.GetCurrentPlayerLoop();
    //     // // minimum is Update | FixedUpdate | LastPostLateUpdate
    //     //PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
    // }

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadCamera = loader.InstantiateAsync(RootAddress.CameraController, inject: false);
        var loadGameSettings = loader.LoadAssetAsync(RootAddress.Settings);
        var loadEventSystem = loader.InstantiateAsync(RootAddress.EventSystem, inject: false);

        LoadDevelopmentAssets(loader);
        (_camera, _gameSettings, _) = await (loadCamera, loadGameSettings, loadEventSystem);
    }

    private static void LoadDevelopmentAssets(IAddressablesLoader loader)
    {
        if (!PlatformInfo.IsDevelopment) return;
        loader.InstantiateAsync(DebugAddress.Graphy, inject: false).Forget();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterCancellationToken(builder);
        RegisterCamera(builder);
        RegisterSettings(builder);

        builder.Register<GameEvents>(Lifetime.Singleton);
        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        builder.Register<PlayerProgress>(Lifetime.Singleton);
    }

    private static void RegisterCancellationToken(IContainerBuilder builder)
    {
        builder.Register<ScopeCancellationToken>(Lifetime.Scoped);
    }

    private void RegisterCamera(IContainerBuilder builder)
    {
        builder.RegisterComponent(_camera);
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _gameSettings.RegisterAllSettings(builder);
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
        builder.Register<IAdBannerShow, AdBannerShow>(Lifetime.Transient);
        builder.Register<BannerAd>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();

        builder.Register<IAdShow, AdShow>(Lifetime.Transient);
        builder.Register<InterstitialAd>(Lifetime.Transient);
        builder.Register<RewardedAd>(Lifetime.Transient);

        builder.Register<IAdInitializer, AdInitializer>(Lifetime.Transient);
        builder.Register<Ads>(Lifetime.Singleton);
    }

    private static void RegisterIAP(IContainerBuilder builder)
    {
        builder.Register<IPurchasingManager, PurchasingManager>(Lifetime.Singleton);
    }
}