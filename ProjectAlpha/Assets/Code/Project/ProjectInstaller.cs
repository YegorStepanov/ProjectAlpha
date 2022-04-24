using Code.Menu;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.Project
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [Required, AssetsOnly, SerializeField]
        private Camera cameraPrefab;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitUniTaskLoop()
        {
            //var loop = PlayerLoop.GetCurrentPlayerLoop();
            // // minimum is Update | FixedUpdate | LastPostLateUpdate
            //PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
        }

        private void Awake() =>
            DOTween.Init();

        public override void InstallBindings()
        {
            RegisterCamera();

            RegisterSceneLoader();

            // RegisterScreenSizeChecker();

            RegisterGameTriggers();

            RegisterAddressableFactory();
        }

        private void RegisterCamera() =>
            Container.Bind<Camera>()
                .FromComponentInNewPrefab(cameraPrefab)
                .WithGameObjectName("Camera")
                .AsSingle()
                .NonLazy();

        private void RegisterSceneLoader() =>
            Container.Bind<SceneLoader>().AsSingle();

        private void RegisterScreenSizeChecker() =>
            Container.BindInterfacesAndSelfTo<ScreenSizeChecker>().AsSingle();

        private void RegisterGameTriggers() =>
            Container.Bind<GameTriggers>().AsSingle();

        private void RegisterAddressableFactory()
        {
            Transform transformInThisScene = transform;
            Container.Bind<AddressableFactory>().AsSingle().WithArguments(transformInThisScene);
        }
    }
}