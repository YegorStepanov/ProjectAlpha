using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Services;
using Code.Services.Game.UI;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : Scope
{
    private IWidthGenerator _widthGenerator;
    private NextPositionGenerator _nextPositionGenerator;
    private HeroController _heroController;
    private IAsyncPool<PlatformController> _platformPool;
    private IAsyncPool<StickController> _stickPool;
    private IAsyncPool<CherryController> _cherryPool;
    private GameUI _gameUI;
    private RedPointHitGameAnimation _redPointHitGameAnimation;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        WidthGeneratorData widthGeneratorData = await loader.LoadAssetAsync(GameAddress.WidthGenerator);
        _widthGenerator = widthGeneratorData.Create();

        _nextPositionGenerator = await loader.LoadAssetAsync(GameAddress.NextPositionGenerator);

        _heroController = await loader.LoadAssetAsync(GameAddress.Hero);

        _platformPool = loader.CreateCyclicPool(GameAddress.Platform, 3, "Platforms");
        _stickPool = loader.CreateCyclicPool(GameAddress.Stick, 2, "Sticks");
        _cherryPool = loader.CreateCyclicPool(GameAddress.Cherry, 2, "Cherries");

        _gameUI = await loader.LoadAssetAsync(GameAddress.GameUI);
        _redPointHitGameAnimation = await loader.LoadAssetAsync(GameAddress.Plus1Notification);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStateMachine>(Lifetime.Singleton);

        builder.RegisterInstance(_widthGenerator);
        builder.RegisterInstance(_nextPositionGenerator); //rework?

        builder.RegisterComponentInNewPrefab(_heroController, Lifetime.Singleton);
        builder.Register<HeroSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_platformPool);
        builder.Register<PlatformSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_stickPool);
        builder.Register<StickSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_cherryPool);
        builder.Register<CherrySpawner>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_gameUI, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_redPointHitGameAnimation, Lifetime.Singleton);

        builder.RegisterEntryPoint<GameStart>();

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        builder.Register<GameData>(Lifetime.Singleton);
        builder.Register<GameSceneLoader>(Lifetime.Singleton);
        builder.Register<GameMediator>(Lifetime.Singleton);
        builder.Register<GameUIController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //temp
        builder.RegisterBuildCallback(resolver =>
        {
            var gum = resolver.Resolve<GameUI>();
            resolver.InjectGameObject(gum.gameObject);

            var gm = resolver.Resolve<GameMediator>();

            gm.gameStateMachine = resolver.Resolve<GameStateMachine>();
        });
    }
}