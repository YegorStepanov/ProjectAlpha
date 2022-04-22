using Code.Game.States;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public sealed class GameInstaller : MonoInstaller
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
            Debug.Log("GameInstaller.InstallBindings");

            Container.BindInstance(this.GetCancellationTokenOnDestroy());

            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();

            Container.BindInstance(widthGenerator);

            Container.BindInstance(SpriteRendererPrefab); //it's bad, ye? move it to own type OR move to settings
            Container.BindInstance(gameSettings);

            // .FromSubContainerResolve().ByInstaller().AsSingle();

            Container.Bind<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();

            Container.Bind<IHeroController>()
                .FromComponentInNewPrefab(hero)
                .WithGameObjectName("Hero")
                .AsSingle();

            Container.BindMemoryPool<PlatformController, PlatformController.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(platform)
                .WithGameObjectName("Platform")
                .UnderTransformGroup("Platforms");

            Container.Bind<IStickController>()
                .FromComponentInNewPrefab(stick)
                .WithGameObjectName("Stick")
                .AsSingle();

            // Container.Bind<GameStateMachineInitializer>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<PlatformSpawner>().AsSingle();

            Container.BindMemoryPool<StickController, StickController.Pool>()
                .WithInitialSize(2)
                .FromComponentInNewPrefab(stick)
                .WithGameObjectName("Stick")
                .UnderTransformGroup("Sticks");

            // Container.BindMemoryPool<IPlatformController, PlatformController.Pool>()
            //     .WithInitialSize(10)
            //     .To<PlatformController>()
            //     .FromComponentInNewPrefab(platform);

            Container.Bind<StickSpawner>().AsSingle();

            Container.Bind<InputManager>().AsSingle();
        }

        [Button]
        public void PlayAnimation() { }
        //=> 
        // Container.Resolve<GameStateMachine>().Enter<GameStartState>();
    }
}