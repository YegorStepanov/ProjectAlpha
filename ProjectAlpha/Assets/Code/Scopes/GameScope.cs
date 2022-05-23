using Code.AddressableAssets;
using Code.Services;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : Scope
{
    private IWidthGenerator _widthGenerator;
    private IPositionGenerator _positionGenerator;
    private HeroController _heroController;
    private IAsyncPool<PlatformController> _platformPool;
    private IAsyncPool<StickController> _stickPool;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        WidthGeneratorData widthGeneratorData = await loader.LoadAssetAsync(GameAddress.WidthGenerator);
        _widthGenerator = widthGeneratorData.Create();

        PositionGeneratorData positionGeneratorData = await loader.LoadAssetAsync(GameAddress.PositionGenerator);
        _positionGenerator = positionGeneratorData.Create();

        _heroController = await loader.LoadAssetAsync(GameAddress.Hero);

        _platformPool = loader.CreateCyclicPool(GameAddress.Platform, 3, "Platforms");
        _stickPool = loader.CreateCyclicPool(GameAddress.Stick, 2, "Sticks");
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStateMachine>(Lifetime.Singleton);

        builder.RegisterInstance(_widthGenerator);
        builder.RegisterInstance(_positionGenerator);

        builder.RegisterComponentInNewPrefab(_heroController, Lifetime.Singleton);
        builder.Register<HeroSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_platformPool);
        builder.Register<PlatformSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(_stickPool);
        builder.Register<StickSpawner>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameStart>();

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
    }
}