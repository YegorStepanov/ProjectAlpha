using System.Threading;
using Code.Game;
using Code.Services;
using Cysharp.Threading.Tasks;
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
    //     DOTween.Init();
    // }

    protected override void Configure(IContainerBuilder builder)
    {
        _gameSettings.RegisterAllSettings(builder);

        //        Container.Bind<AddressableFactory>().AsSingle().WithArguments(transform);

        builder.RegisterComponentInNewPrefab(_cameraController, Lifetime.Singleton); //GOname? nonlazy?

        builder.Register<SceneLoader>(Lifetime.Singleton);
        builder.Register<GameTriggers>(Lifetime.Singleton);
        builder.Register<InputManager>(Lifetime.Singleton);
        
        builder.Register<StartSceneInformer>(Lifetime.Singleton).Build(); //non-lazy

        //// builder.RegisterInstance(transform);
        builder.Register<AddressableFactory>(Lifetime.Singleton);

        var rootToken = new RootCancellationToken(this.GetCancellationTokenOnDestroy());
        builder.RegisterInstance(rootToken);
    }
}

public record struct RootCancellationToken(CancellationToken Token);
