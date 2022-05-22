using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Cysharp.Threading.Tasks;
using Tayx.Graphy;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

//todo: create issue "No reference checker when CodeGen is enabled"
public sealed class RootScope : Scope
{
    private CameraController _cameraController;
    private GameSettings _gameSettings;
    private GraphyManager _graphy;

    // todo:
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    // public static void InitUniTaskLoop()
    // {
    //     //var loop = PlayerLoop.GetCurrentPlayerLoop();
    //     // // minimum is Update | FixedUpdate | LastPostLateUpdate
    //     //PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
    // }

    // protected override void Awake()
    // {
    //     base.Awake();
    //     
    // }

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        _cameraController = await loader.InstantiateAsync(RootAddress.CameraController);
        _gameSettings = await loader.LoadAssetAsync(RootAddress.Settings);
        _graphy = await loader.InstantiateAsync(RootAddress.Graphy);
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

        builder.RegisterComponent(_cameraController);
        builder.RegisterInstance(_graphy);
        _gameSettings.RegisterAllSettings(builder);

        builder.RegisterInstance(SceneLoader.Instance);
        builder.Register<GameTriggers>(Lifetime.Singleton);
        builder.Register<InputManager>(Lifetime.Singleton);

        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        // to inject dependencies, it should be Scoped with static instances
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);

        builder.RegisterEntryPoint<RootStart>();
    }
}