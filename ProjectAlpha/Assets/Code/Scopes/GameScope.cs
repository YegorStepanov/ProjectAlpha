using Code.Services;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : LifetimeScope
{
    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(WidthGenerator))]
    private WidthGenerator _widthGenerator; //split to settings and own generator?

    public HeroController _hero;

    private readonly Address<PlatformController> _platform = new("Platform");
    private readonly Address<StickController> _stick = new("Stick");

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterPlatformPool(builder);
        RegisterStickPool(builder);

        builder.Register<GameStateMachine>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_hero, Lifetime.Singleton)
            .As<IHeroController>();

        builder.RegisterComponent(Instantiate(_widthGenerator));

        builder.RegisterEntryPoint<GameStart>();

        builder.Register<StickSpawner>(Lifetime.Singleton);
        builder.Register<PlatformSpawner>(Lifetime.Singleton);

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
        
        //RegisterComponent = RegisterInstance + Resolve NonLazy?
    }

    private void RegisterPlatformPool(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(_platform, "Platforms", 0, 3, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<PlatformController>, RecyclablePool<PlatformController>>
            (Lifetime.Singleton);
    }

    private void RegisterStickPool(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(_stick, "Sticks", 0, 2, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<StickController>, RecyclablePool<StickController>>
            (Lifetime.Singleton);
    }
}