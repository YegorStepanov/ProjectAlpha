using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Services;
using Code.Services.Game.UI;
using Code.States;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : Scope
{
    private IWidthGenerator _widthGenerator;
    private PlatformPositionGenerator _platformPositionGenerator;
    private CherryPositionGenerator _cherryPositionGenerator;

    private Hero _hero;
    private IAsyncPool<Platform> _platformPool;
    private IAsyncPool<Stick> _stickPool;
    private IAsyncPool<Cherry> _cherryPool;
    private GameUI _gameUI;
    private RedPointHitGameAnimation _redPointHitGameAnimation;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        WidthGeneratorData widthGeneratorData = await loader.LoadAssetAsync(GameAddress.WidthGenerator);
        _widthGenerator = widthGeneratorData.Create();

        _platformPositionGenerator = await loader.LoadAssetAsync(GameAddress.PlatformPositionGenerator);
        _cherryPositionGenerator = await loader.LoadAssetAsync(GameAddress.CherryPositionGenerator);

        _hero = await loader.LoadAssetAsync(GameAddress.Hero);

        _platformPool = loader.CreatePool(GameAddress.Platform, 0, 3, "Platforms");

        _platformPool = loader.CreateCyclicPool(GameAddress.Platform, 0, 3, "Platforms");
        _stickPool = loader.CreateCyclicPool(GameAddress.Stick, 0, 2, "Sticks");
        _cherryPool = loader.CreateCyclicPool(GameAddress.Cherry, 0, 2, "Cherries");

        _gameUI = await loader.LoadAssetAsync(GameAddress.GameUI);
        _redPointHitGameAnimation = await loader.LoadAssetAsync(GameAddress.Plus1Notification);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        builder.Register<GameWorld>(Lifetime.Singleton); //move to

        RegisterHero(builder);
        RegisterPlatform(builder);
        RegisterStick(builder);
        RegisterCherry(builder);

        RegisterUI(builder);
        RegisterGameStateMachine(builder);

        builder.RegisterEntryPoint<GameEntryPoint>();

        builder.RegisterBuildCallback(resolver =>
        {
            var gum = resolver.Resolve<GameUI>();
            resolver.InjectGameObject(gum.gameObject);

            var gm = resolver.Resolve<GameMediator>();

            gm.gameStateMachine = resolver.Resolve<GameStateMachine>();
        });
    }

    private void RegisterHero(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_hero, Lifetime.Singleton);
        builder.Register<IHeroAnimations, HeroAnimations>(Lifetime.Singleton);
        builder.Register<HeroSpawner>(Lifetime.Singleton);
    }

    private void RegisterPlatform(IContainerBuilder builder)
    {
        builder.RegisterInstance(_platformPool);
        builder.Register<IPlatformAnimations, PlatformAnimations>(Lifetime.Singleton);
        builder.Register<PlatformSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_widthGenerator);
        builder.RegisterInstance(_platformPositionGenerator);
    }

    private void RegisterStick(IContainerBuilder builder)
    {
        builder.RegisterInstance(_stickPool);
        builder.Register<IStickAnimations, StickAnimations>(Lifetime.Singleton);
        builder.Register<StickSpawner>(Lifetime.Singleton);
    }

    private void RegisterCherry(IContainerBuilder builder)
    {
        builder.RegisterInstance(_cherryPool);
        builder.Register<ICherryAnimations, CherryAnimations>(Lifetime.Singleton);
        builder.Register<CherrySpawner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.RegisterInstance(_cherryPositionGenerator);
    }

    private void RegisterUI(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_gameUI, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_redPointHitGameAnimation, Lifetime.Singleton);

        builder.Register<GameSceneLoader>(Lifetime.Singleton);
        builder.Register<GameMediator>(Lifetime.Singleton);
        builder.Register<GameUIController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }

    private static void RegisterGameStateMachine(IContainerBuilder builder)
    {
        builder.Register<IExitState, StartState>(Lifetime.Transient);
        builder.Register<IExitState, MoveHeroToPlatformState>(Lifetime.Transient);
        builder.Register<IExitState, NextRoundState>(Lifetime.Transient);
        builder.Register<IExitState, StickControlState>(Lifetime.Transient);
        builder.Register<IExitState, RestartState>(Lifetime.Transient);
        builder.Register<IExitState, MoveHeroToGameOverState>(Lifetime.Transient);

        builder.Register<GameStateMachine>(Lifetime.Singleton);
    }
}
