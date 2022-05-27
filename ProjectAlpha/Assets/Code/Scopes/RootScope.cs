using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class RootScope : Scope
{
    private CameraController _camera;
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
        var loadGraphy = loader.InstantiateAsync(RootAddress.Graphy, inject: false);
        var loadEventSystem = loader.InstantiateAsync(RootAddress.EventSystem, inject: false);

        (_camera, _gameSettings, _, _) = await (loadCamera, loadGameSettings, loadGraphy, loadEventSystem);
    }

    protected override void Configure(IContainerBuilder builder)
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

        builder.RegisterInstance(SceneLoader.Instance);
        builder.Register<GameTriggers>(Lifetime.Singleton);
        builder.Register<InputManager>(Lifetime.Singleton);

        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);

        builder.RegisterBuildCallback(BuildCallback);
    }

    private static void BuildCallback(IObjectResolver resolver)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);
    }
}