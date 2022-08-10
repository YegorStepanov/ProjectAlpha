using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Common;
using Code.Data;
using Code.Extensions;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Navigators;
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

    private IAsyncPool<Hero> _heroPool;
    private IAsyncPool<Platform> _platformPool;
    private IAsyncPool<Stick> _stickPool;
    private IAsyncPool<Cherry> _cherryPool;
    private GameUIView _gameUIView;
    private RedPointHitGameAnimation _redPointHitGameAnimation;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        await using UniTaskDisposable _ = Parent.Container.Resolve<CameraBackground>()
            .ChangeBackgroundAsync();

        var tasks = UniTask.WhenAll(
            loader.LoadAssetAsync(Address.Data.WidthGenerator),
            loader.LoadAssetAsync(Address.Data.PlatformPositionGenerator),
            loader.LoadAssetAsync(Address.Data.CherryPositionGenerator),
            loader.InstantiateAsync(Address.UI.GameUI),
            loader.InstantiateAsync(Address.UI.RedPointHitAnimation));

        Address<Hero> hero = Parent.Container.Resolve<HeroSelector>()
            .GetSelectedHero();

        _heroPool = loader.CreateRecyclableAddressablePool(hero, 1, 1);
        _platformPool = loader.CreateRecyclableAddressablePool(Address.Entity.Platform, 0, 3);
        _stickPool = loader.CreateRecyclableAddressablePool(Address.Entity.Stick, 0, 2);
        _cherryPool = loader.CreateRecyclableAddressablePool(Address.Entity.Cherry, 0, 2);

        (var widthGeneratorData,
            _platformPositionGenerator,
            _cherryPositionGenerator,
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
        RegisterGameStates(builder);

        RegisterGameServices(builder);

        builder.RegisterEntryPoint<GameEntryPoint>();
    }

    private void RegisterHero(IContainerBuilder builder)
    {
        builder.RegisterInstance(_heroPool);
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
        builder.Register<CherrySpawner>(Lifetime.Singleton).As<ICherryPickHandler>().AsSelf();

        builder.RegisterInstance(_cherryPositionGenerator);
    }

    private void RegisterUI(IContainerBuilder builder)
    {
        builder.RegisterComponentAndInjectGameObject(_gameUIView);
        builder.RegisterComponent(_redPointHitGameAnimation);

        builder.Register<IGameUIFacade, GameUIFacade>(Lifetime.Singleton);
        builder.Register<GameUIController>(Lifetime.Singleton);
    }

    private static void RegisterGameStates(IContainerBuilder builder)
    {
        builder.Register<IExitState, StartState>(Lifetime.Singleton);
        builder.Register<IExitState, HeroMovementToStartPlatformState>(Lifetime.Singleton);
        builder.Register<IExitState, HeroMovementToPlatformState>(Lifetime.Singleton);
        builder.Register<IExitState, NextRoundState>(Lifetime.Singleton);
        builder.Register<IExitState, CameraMovementState>(Lifetime.Singleton);
        builder.Register<IExitState, EntitiesMovementState>(Lifetime.Singleton);
        builder.Register<IExitState, StickControlState>(Lifetime.Singleton);
        builder.Register<IExitState, RestartState>(Lifetime.Singleton);
        builder.Register<IExitState, HeroMovementToEndGameState>(Lifetime.Singleton);
        builder.Register<IExitState, EndGameState>(Lifetime.Singleton);

        builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
    }

    private static void RegisterGameServices(IContainerBuilder builder)
    {
        builder.Register<GameHeightFactory>(Lifetime.Singleton);
        builder.Register<GameStartEventAwaiter>(Lifetime.Singleton);
        builder.Register<GameStateResetter>(Lifetime.Singleton);
        builder.Register<SpawnersResetter>(Lifetime.Singleton);
        builder.Register<SpawnersItemsMover>(Lifetime.Singleton);
        builder.Register<HeroMovement>(Lifetime.Singleton);
        builder.Register<ICameraRestorer, CameraRestorer>(Lifetime.Singleton).As<IStartable>();
        builder.Register<IGameSceneNavigator, GameSceneNavigator>(Lifetime.Singleton);
    }
}
