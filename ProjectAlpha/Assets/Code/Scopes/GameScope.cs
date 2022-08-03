using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Data;
using Code.Extensions;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Code.Services.UI;
using Code.States;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : Scope
{
    private IPlatformWidthGenerator _platformWidthGenerator;
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
        Address<Hero> hero = Parent.Container.Resolve<HeroSelector>().GetSelectedHero();

        var tasks = UniTask.WhenAll(
            loader.LoadAssetAsync(Address.Data.WidthGenerator),
            loader.LoadAssetAsync(Address.Data.PlatformPositionGenerator),
            loader.LoadAssetAsync(Address.Data.CherryPositionGenerator),
            loader.LoadAssetAsync(hero),
            loader.LoadAssetAsync(Address.UI.GameUI),
            loader.LoadAssetAsync(Address.UI.RedPointHitAnimation));

        _platformPool = loader.CreateRecyclableAddressablePool(Address.Entity.Platform, 0, 3);
        _stickPool = loader.CreateRecyclableAddressablePool(Address.Entity.Stick, 0, 2);
        _cherryPool = loader.CreateRecyclableAddressablePool(Address.Entity.Cherry, 0, 2);

        (PlatformWidthGeneratorData widthGeneratorData,
            _platformPositionGenerator,
            _cherryPositionGenerator,
            _hero,
            _gameUIView,
            _redPointHitGameAnimation) = await tasks;

        _platformWidthGenerator = widthGeneratorData.Create();
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        RegisterHero(builder);
        RegisterPlatform(builder);
        RegisterStick(builder);
        RegisterCherry(builder);

        RegisterUI(builder);
        RegisterGameStateMachine(builder);

        RegisterGameServices(builder);

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

        builder.RegisterInstance(_platformWidthGenerator);
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
        builder.InjectGameObject(_gameUIView);

        builder.RegisterComponentInNewPrefab(_redPointHitGameAnimation, Lifetime.Singleton);

        builder.Register<IGameSceneNavigator, GameSceneNavigator>(Lifetime.Singleton);
        builder.Register<IGameUIFacade, GameUIFacade>(Lifetime.Singleton);
        builder.Register<GameUIController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }

    private static void RegisterGameStateMachine(IContainerBuilder builder)
    {
        builder.Register<IExitState, StartState>(Lifetime.Singleton);
        builder.Register<IExitState, MoveHeroToMenuPlatformState>(Lifetime.Singleton);
        builder.Register<IExitState, MoveHeroToPlatformState>(Lifetime.Singleton);
        builder.Register<IExitState, NextRoundState>(Lifetime.Singleton);
        builder.Register<IExitState, StickControlState>(Lifetime.Singleton);
        builder.Register<IExitState, RestartState>(Lifetime.Singleton);
        builder.Register<IExitState, MoveHeroToGameOverState>(Lifetime.Singleton);
        builder.Register<IExitState, EndGameState>(Lifetime.Singleton);

        builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
    }

    private static void RegisterGameServices(IContainerBuilder builder)
    {
        builder.Register<GameWorld>(Lifetime.Singleton);
        builder.Register<GameStartEventAwaiter>(Lifetime.Singleton);
        builder.Register<GameStateResetter>(Lifetime.Singleton);
        builder.Register<SpawnersResetter>(Lifetime.Singleton);
        builder.Register<HeroMovement>(Lifetime.Singleton);
        builder.Register<CameraMover>(Lifetime.Singleton);
    }
}
