using System.Diagnostics;
using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Code.Services.Game.UI;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
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

    [Conditional("DEVELOPMENT")]
    private static void LoadDevelopmentAssets(IAddressablesLoader loader) =>
        loader.InstantiateAsync(DebugAddress.Graphy, inject: false).Forget();

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        //when fail, erro contain usefull info:
        //https://docs.unity3d.com/Packages/com.unity.addressables@1.20/manual/LoadingAssetBundles.html

        //long size = await Addressables.GetDownloadSizeAsync(key);
        //if size > 0 
        //Addressables.DownloadDependenciesAsync(key);

        //AsyncOperationHandle.PercentComplete
        //AsyncOperationHandle.GetDownloadStatus
        //Addressables.GetDownloadSizeAsync() == 0 if it cached

        builder.RegisterComponent(_camera);
        _gameSettings.RegisterAllSettings(builder);

        builder.RegisterInstance<ISceneLoader>(SceneLoader.Instance);
        builder.Register<GameEvents>(Lifetime.Singleton);
        builder.Register<InputManager>(Lifetime.Singleton);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);

        RegisterAds(builder);
        RegisterIAP(builder);

        builder.Register<GameData>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf(); //move to

        builder.Register<PlayerProgress>(Lifetime.Singleton);

        builder.RegisterBuildCallback(BuildCallback);
    }

    private void RegisterAds(IContainerBuilder builder)
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

    private static void BuildCallback(IObjectResolver resolver)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);
    }
}