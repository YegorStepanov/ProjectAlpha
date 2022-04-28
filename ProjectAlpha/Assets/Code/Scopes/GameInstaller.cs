using Code.Game;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Scopes
{
    public sealed class GameInstaller : BaseInstaller<GameInitializer>
    {
        [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(WidthGenerator))]
        private WidthGenerator widthGenerator; //split to settings and own generator?

        [SerializeField]
        private SpriteRenderer SpriteRendererPrefab;

        [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(GameSettings))]
        private GameSettings gameSettings;

        public HeroController hero;

        public StickController stick;

        [AssetsOnly]
        public PlatformController platform;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            

            RegisterWidthGenerator();

            RegisterSpriteRenderer();
            RegisterGameSettings();

            RegisterGameStateMachine();

            RegisterHeroController();

            RegisterPlatformControllerPool();

            RegisterStickController();

            RegisterPlatformSpawner();

            RegisterStickControllerPool();

            RegisterStickSpawner();
        }
        
        private void RegisterStickSpawner() =>
            Container.Bind<StickSpawner>().AsSingle();

        private void RegisterStickControllerPool() =>
            Container.BindMemoryPool<StickController, StickController.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(stick)
                .WithGameObjectName("Stick")
                .UnderTransformGroup("Sticks");

        private void RegisterPlatformSpawner() =>
            Container.Bind<PlatformSpawner>().AsSingle();

        private void RegisterStickController()
        {
            Container.Bind<IStickController>()
                .FromComponentInNewPrefab(stick)
                .WithGameObjectName("Stick")
                .AsSingle();
        }

        private void RegisterPlatformControllerPool()
        {
            Container.BindMemoryPool<PlatformController, PlatformController.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(platform)
                .WithGameObjectName("Platform")
                .UnderTransformGroup("Platforms");
        }

        private void RegisterHeroController() =>
            Container.Bind<IHeroController>()
                .FromComponentInNewPrefab(hero)
                .WithGameObjectName("Hero")
                .AsSingle();

        private void RegisterGameStateMachine() =>
            Container.Bind<GameStateMachine>().AsSingle();

        private void RegisterGameSettings() =>
            Container.BindInstance(gameSettings);

        
        private void RegisterSpriteRenderer() => //wtf? it's bad, ye? move it to own type OR move to settings
            Container.BindInstance(SpriteRendererPrefab);

        private void RegisterWidthGenerator() =>
            Container.BindInstance(widthGenerator);

    }
}