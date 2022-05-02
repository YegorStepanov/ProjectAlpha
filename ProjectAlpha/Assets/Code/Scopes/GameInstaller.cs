using Code.Game;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Scopes;

public sealed class GameInstaller : BaseInstaller<GameInitializer>
{
    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(WidthGenerator))]
    private WidthGenerator _widthGenerator; //split to settings and own generator?

    public HeroController _hero;

    public StickController _stick;

    [AssetsOnly]
    public PlatformController _platform;

    public override void InstallBindings()
    {
        base.InstallBindings();

        RegisterGameStateMachine();

        RegisterHeroController();

        RegisterPlatformSpawner();

        RegisterStickSpawner();

        RegisterWidthGenerator();
    }

    private void RegisterGameStateMachine() =>
        Container.Bind<GameStateMachine>().AsSingle();

    private static void RegisterHeroController(DiContainer container)
    {
        //todo: convert to local function, it's faster?
        //check it in sharplab
        container.BindAsync<IHeroController>().FromMethod(async () =>
        {
            var factory = container.Resolve<AddressableFactory>();
            await Task.Delay(100);

            GameObject hero = await factory.InstantiateAsync("Hero");
            container.InjectGameObject(hero);
            hero.name = "MyHero";
            return hero.GetComponent<HeroController>();
        }).AsSingle();

        //container.Bind<IHeroController>()
        //    .FromComponentInNewPrefab(hero)
        //    .WithGameObjectName("Hero")
        //    .AsSingle();
    }
    
    private void RegisterStickSpawner()
    {
        Container.BindMemoryPool<StickController, StickController.Pool>()
            .WithInitialSize(2) //move it to settings
            .FromComponentInNewPrefab(_stick)
            .WithGameObjectName("Stick")
            .UnderTransformGroup("Sticks");

        Container.Bind<StickSpawner>().AsSingle();
    }

    private void RegisterPlatformSpawner()
    {
        Container.BindMemoryPool<PlatformController, PlatformController.Pool>()
            .WithInitialSize(5) //move it to settings
            .FromComponentInNewPrefab(_platform)
            .WithGameObjectName("Platform")
            .UnderTransformGroup("Platforms");

        Container.Bind<PlatformSpawner>().AsSingle();
    }

    private void RegisterWidthGenerator() =>
        Container.Bind<WidthGenerator>()
            .To<PercentageWidthGenerator>()
            .FromNewScriptableObject(_widthGenerator)
            .AsSingle();
}