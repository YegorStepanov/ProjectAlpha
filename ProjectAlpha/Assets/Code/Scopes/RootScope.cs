using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

//todo: create issue "No reference checker when CodeGen is enabled"
public sealed class RootScope : LifetimeScope
{
    [Required, AssetsOnly, SerializeField]
    private CameraController _cameraController;

    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(GameSettings))]
    private GameSettings _gameSettings;

    [Required, SerializeField]
    private GameObject _graphy;

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

    protected override async void Configure(IContainerBuilder builder)
    {
        // var q = await Addressables.LoadResourceLocationsAsync()

        //Addressables.InstantiateAsync() == Addressables.ReleaseInstance
        //Addressables.LoadAsset() == Addressables.Release()

        //asset bundles:
        //AssetBundle.LoadFromFileAsync - local
        //UnityWebRequest.GetAssetBundle - remote

        //when fail, erro contain usefull info:
        //https://docs.unity3d.com/Packages/com.unity.addressables@1.20/manual/LoadingAssetBundles.html

        //long size = await Addressables.GetDownloadSizeAsync(key);
        //if size > 0 
        //Addressables.DownloadDependenciesAsync(key);

        //AsyncOperationHandle.PercentComplete
        //AsyncOperationHandle.GetDownloadStatus
        //Addressables.GetDownloadSizeAsync() == 0 if it cached

        //if u don't need to access the result, set autoReleaseHandle to true:
        //Addressables.DownloadDependenciesAsync("preload", true);

        // Addressables.DownloadDependenciesAsync()

        //how to check if it remote?

        //to Event Viewer works correct, simply

        //refactor it
        Instantiate(_graphy);

        _gameSettings.RegisterAllSettings(builder);

        //        Container.Bind<AddressableFactory>().AsSingle().WithArguments(transform);

        builder.RegisterComponentInNewPrefab(_cameraController, Lifetime.Singleton); //GOname? nonlazy?

        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        builder.Register<GameTriggers>(Lifetime.Singleton);
        builder.Register<InputManager>(Lifetime.Singleton);

        builder.Register<StartSceneInformer>(Lifetime.Singleton); //.Build(); //non-lazy

        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        // to inject dependencies, it should be Scoped with static instances
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);

        builder.RegisterEntryPoint<RootStart>();
    }
}