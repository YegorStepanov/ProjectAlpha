using Code.Game1;
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

        private void Awake()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EditorOnly"))
                Destroy(go);
        }

        public override void InstallBindings()
        {
            Debug.Log("GameInstaller.InstallBindings");
            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();

            Container.BindInstance(platformWidthGenerator);

            Container.BindInstance(SpriteRendererPrefab); //it's bad, ye? move it to own type OR move to settings
            Container.BindInstance(gameSettings);

            Container.Bind<PlatformCreator>().AsSingle();
            // .FromSubContainerResolve().ByInstaller().AsSingle();

            Container.BindInterfacesTo<GameInitializer>().AsSingle();
        }

        [Button]
        public void PlayAnimation()
        {
            var cameraService = Container.Resolve<CameraService>();
            cameraService.Move();
        }
    }
}