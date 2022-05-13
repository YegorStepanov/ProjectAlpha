using Code.Services;
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

    public StickController _stick;

    [AssetsOnly]
    public PlatformController _platform;

    protected override void Configure(IContainerBuilder builder)
    {
        //consider: .WithName("Platform").WithPrefabName().UnderContainer("Platforms").WithInitialSize(2)

        // Addressables.asset

        // builder.RegisterMonoBehaviourPool(_platform, "Platform", "Platforms", 0, 3, Lifetime.Singleton);
        var platformPrefab = new Address<PlatformController>("Platform");
        builder.RegisterAddressablePool(platformPrefab, "Platforms", 0, 3, Lifetime.Singleton);

        builder.RegisterMonoBehaviourPool(_stick, "Stick", "Sticks", 0, 2, Lifetime.Singleton);

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
}