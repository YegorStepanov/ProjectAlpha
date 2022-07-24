using System;
using System.Threading;
using Code.AddressableAssets;
using Code.Game;
using Code.Infrastructure;
using Code.Services;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Camera = Code.Services.Camera;
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
        RegisterCancellationToken(builder);
        RegisterCamera(builder);

        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        RegisterProgress(builder);

        builder.RegisterEntryPoint<RootEntryPoint>();

        RegisterMessagePipe(builder);
    }

    private static void RegisterProgress(IContainerBuilder builder)
    {
        builder.Register<IPersistentProgress, PersistentProgress>(Lifetime.Singleton);
        builder.Register<ISessionProgress, SessionProgress>(Lifetime.Singleton);
        builder.Register<IProgress, Progress>(Lifetime.Singleton);
    }

    private static void RegisterMessagePipe(IContainerBuilder builder)
    {
        // RegisterMessagePipe returns options.
        var options = builder.RegisterMessagePipe( /* configure option */);

        // Setup GlobalMessagePipe to enable diagnostics window and global function
        builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

        // RegisterMessageBroker: Register for IPublisher<T>/ISubscriber<T>, includes async and buffered.
        builder.RegisterMessageBroker<int>(options);
        builder.RegisterMessageBroker<Event.GameStart>(options);

        // also exists RegisterMessageBroker<TKey, TMessage>, RegisterRequestHandler, RegisterAsyncRequestHandler

        // RegisterMessageHandlerFilter: Register for filter, also exists RegisterAsyncMessageHandlerFilter, Register(Async)RequestHandlerFilter
        //builder.RegisterMessageHandlerFilter<MyFilter<int>>();

        builder.RegisterEntryPoint<MessagePipeDemo>(Lifetime.Singleton);
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _settingsFacade.RegisterSettings(builder);
    }

    private static void RegisterCancellationToken(IContainerBuilder builder)
    {
        //builder.Register<CancellationToken>(Lifetime.Scoped);
        Func<IObjectResolver, Func<LifetimeScope, CancellationToken>> factoryFactory;
        // builder.RegisterFactory<LifetimeScope, CancellationToken>(factoryFactory, Lifetime.Scoped);
        // builder.RegisterFactory<LifetimeScope, CancellationToken>(Factory, Lifetime.Scoped);

        // builder.RegisterFactory<LifetimeScope, CancellationToken>(_ => scope =>
        // {
        //     Debug.Log("NewFactory!!!");
        //     return scope.GetCancellationTokenOnDestroy();
        // }, Lifetime.Scoped);
        //
        // builder.RegisterFactory<LifetimeScope, CancellationToken>(scope => scope.GetCancellationTokenOnDestroy());
    }

    // private static Func<LifetimeScope, CancellationToken> Factory(IObjectResolver resolver)
    // {
    //     return scope => scope.GetCancellationTokenOnDestroy();
    // }
    //
    // public static RegistrationBuilder RegisterFactory<TParam1, T>(
    //     this IContainerBuilder builder,
    //     Func<IObjectResolver, Func<TParam1, T>> factoryFactory,
    //     Lifetime lifetime)
    //     => builder.Register(new FuncRegistrationBuilder(factoryFactory, typeof(Func<TParam1, T>), lifetime));
    //
    // public static RegistrationBuilder RegisterFactory<TParam1, T>(
    //     this IContainerBuilder builder,
    //     Func<TParam1, T> factory)
    //     => builder.RegisterInstance(factory);

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
        //builder.RegisterFactory()
        builder.Register<IIAPManager, IAPManager>(Lifetime.Singleton);
    }
}

public class MessagePipeDemo : IStartable
{
    private readonly IPublisher<int> _publisher;
    private readonly ISubscriber<int> _subscriber;

    public MessagePipeDemo(IPublisher<int> publisher, ISubscriber<int> subscriber)
    {
        _publisher = publisher;
        _subscriber = subscriber;
    }

    public void Start()
    {
        return;
        DisposableBagBuilder d = DisposableBag.CreateBuilder();
        _subscriber.Subscribe(x => Debug.Log("S1:" + x)).AddTo(d);
        _subscriber.Subscribe(x => Debug.Log("S2:" + x)).AddTo(d);

        ISubscriber<int> a1;
        a1.Subscribe(i => { });

        IAsyncSubscriber<int> a2;
        //a2..Subscribe((i, token) => { });

        IBufferedAsyncSubscriber<int> a3;
        //a3.SubscribeAsync((i, token) => {});

        _publisher.Publish(10);
        _publisher.Publish(20);
        _publisher.Publish(30);

        var disposable = d.Build();
        disposable.Dispose();
    }

    private UniTask Handler(int arg1, CancellationToken arg2)
    {
        throw new System.NotImplementedException();
    }
}
