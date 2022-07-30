using Code.AddressableAssets.Loaders;
using Code.AddressableAssets.Pools.Async;
using Code.Animations.Game;
using Code.Data.PositionGenerator;
using Code.Data.WidthGenerator;
using Code.Extensions;
using Code.Scopes.EntryPoints;
using Code.Services;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Entities.Stick;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Code.Services.UI.Game;
using Code.States;
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
    private GameUIView _gameUIView;
    private RedPointHitGameAnimation _redPointHitGameAnimation;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        WidthGeneratorData widthGeneratorData = await loader.LoadAssetAsync(Address.Data.WidthGenerator);
        _widthGenerator = widthGeneratorData.Create();

        _platformPositionGenerator = await loader.LoadAssetAsync(Address.Data.PlatformPositionGenerator);
        _cherryPositionGenerator = await loader.LoadAssetAsync(Address.Data.CherryPositionGenerator);

        _hero = await loader.LoadAssetAsync(Address.Entity.Hero);

        _platformPool = loader.CreatePool(Address.Entity.Platform, 0, 3, "Platforms");

        _platformPool = loader.CreateCyclicPool(Address.Entity.Platform, 0, 3, "Platforms");
        _stickPool = loader.CreateCyclicPool(Address.Entity.Stick, 0, 2, "Sticks");
        _cherryPool = loader.CreateCyclicPool(Address.Entity.Cherry, 0, 2, "Cherries");

        _gameUIView = await loader.LoadAssetAsync(Address.UI.GameUI);
        _redPointHitGameAnimation = await loader.LoadAssetAsync(Address.UI.Plus1Notification);
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.Register<GameWorld>(Lifetime.Singleton); //move to

        RegisterHero(builder);
        RegisterPlatform(builder);
        RegisterStick(builder);
        RegisterCherry(builder);

        RegisterUI(builder);
        RegisterGameStateMachine(builder);

        builder.RegisterEntryPoint<GameEntryPoint>();
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
        builder.RegisterComponentInNewPrefab(_gameUIView, Lifetime.Singleton);
        builder.InjectGameObject<GameUIView>();

        builder.RegisterComponentInNewPrefab(_redPointHitGameAnimation, Lifetime.Singleton);

        builder.Register<IGameSceneNavigator, GameSceneNavigator>(Lifetime.Singleton);
        builder.Register<IGameUIActions, GameUIActions>(Lifetime.Singleton);
        builder.Register<GameUIController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }

    private static void RegisterGameStateMachine(IContainerBuilder builder)
    {
        builder.Register<HeroMovement>(Lifetime.Singleton);
        builder.Register<CameraMover>(Lifetime.Singleton);

        builder.Register<IExitState, StartState>(Lifetime.Transient);
        builder.Register<IExitState, MoveHeroToMenuPlatformState>(Lifetime.Transient);
        builder.Register<IExitState, MoveHeroToPlatformState>(Lifetime.Transient);
        builder.Register<IExitState, NextRoundState>(Lifetime.Transient);
        builder.Register<IExitState, StickControlState>(Lifetime.Transient);
        builder.Register<IExitState, RestartState>(Lifetime.Transient);
        builder.Register<IExitState, MoveHeroToGameOverState>(Lifetime.Transient);
        builder.Register<IExitState, EndGameState>(Lifetime.Transient);

        builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);

        builder.Register<GameStartEventAwaiter>(Lifetime.Singleton);
        builder.Register<GameStateResetter>(Lifetime.Singleton);
        builder.Register<SpawnersResetter>(Lifetime.Singleton);
    }
}
