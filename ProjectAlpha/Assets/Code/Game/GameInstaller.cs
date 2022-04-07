using Code.Annotations;
using Code.Game.States;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public sealed class GameInstaller : MonoInstaller
    {
        [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(PlatformWidthGenerator))]
        private PlatformWidthGenerator platformWidthGenerator; //split to settings and own generator?

        [SerializeField]
        private SpriteRenderer SpriteRendererPrefab;

        [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(GameSettings))]
        private GameSettings gameSettings;

        public HeroController hero;
        
        [AssetsOnly]
        public PlatformController platform;

        public override void InstallBindings()
        {
            Debug.Log("GameInstaller.InstallBindings");
            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();

            Container.BindInstance(platformWidthGenerator);

            Container.BindInstance(SpriteRendererPrefab); //it's bad, ye? move it to own type OR move to settings
            Container.BindInstance(gameSettings);

            // .FromSubContainerResolve().ByInstaller().AsSingle();

            Container.Bind<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();

            Container.Bind<IHeroController>()
                .FromComponentInNewPrefab(hero)
                .AsSingle();

            Container.BindMemoryPool<PlatformController, PlatformController.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(platform)
                .UnderTransformGroup("Platforms");

            
            // Container.BindMemoryPool<IPlatformController, PlatformController.Pool>()
            //     .WithInitialSize(10)
            //     .To<PlatformController>()
            //     .FromComponentInNewPrefab(platform);

            Container.Bind<PlatformSpawner>().AsSingle();
        }

        [Button]
        public void PlayAnimation() { } 
        //=> 
        // Container.Resolve<GameStateMachine>().Enter<GameStartState>();
    }
}