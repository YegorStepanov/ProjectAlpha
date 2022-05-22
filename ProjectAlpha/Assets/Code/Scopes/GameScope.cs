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

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        WidthGeneratorData widthGeneratorData = await loader.LoadAssetAsync(GameAddress.WidthGenerator);
        _widthGenerator = widthGeneratorData.Create();

        PositionGeneratorData positionGeneratorData = await loader.LoadAssetAsync(GameAddress.PositionGenerator);
        _positionGenerator = positionGeneratorData.Create();

        _heroController = await loader.LoadAssetAsync(GameAddress.Hero);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterPlatformSpawner(builder);
        RegisterStickSpawner(builder);

        builder.Register<GameStateMachine>(Lifetime.Singleton);

        builder.RegisterInstance(_widthGenerator);
        builder.RegisterInstance(_positionGenerator);
        builder.RegisterComponent(this.InstantiateInScene(_heroController));

        builder.Register<HeroSpawner>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameStart>();

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        //RegisterComponent = RegisterInstance + Resolve NonLazy?
    }

    private static void RegisterPlatformSpawner(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(GameAddress.Platform, "Platforms", 0, 3, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<PlatformController>, RecyclablePool<PlatformController>>
            (Lifetime.Singleton);

        builder.Register<PlatformSpawner>(Lifetime.Singleton);
    }

    private static void RegisterStickSpawner(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(GameAddress.Stick, "Sticks", 0, 2, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<StickController>, RecyclablePool<StickController>>
            (Lifetime.Singleton);

        builder.Register<StickSpawner>(Lifetime.Singleton);
    }
}