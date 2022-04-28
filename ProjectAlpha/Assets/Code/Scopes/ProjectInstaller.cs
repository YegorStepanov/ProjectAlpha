using Code.Services;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Scopes
{
    public sealed class ProjectInstaller : BaseInstaller<ProjectInitializer>
    {
        [Required, AssetsOnly, SerializeField]
        private CameraController CameraController;
        
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
            base.InstallBindings();
            
            RegisterSceneLoader();

            RegisterCamera();
            // RegisterScreenSizeChecker();

            RegisterGameTriggers();

            RegisterInputManager();

            Container.Bind<StartSceneInformer>().AsSingle().NonLazy();
        }
        
        private void RegisterCamera() =>
            Container.Bind<CameraController>()
                .FromComponentInNewPrefab(CameraController)
                .WithGameObjectName("Camera")
                .AsSingle()
                .NonLazy();

        private void RegisterSceneLoader() =>
            Container.Bind<SceneLoader>().AsSingle();

        private void RegisterScreenSizeChecker() =>
            Container.BindInterfacesAndSelfTo<ScreenSizeChecker>().AsSingle();

        private void RegisterGameTriggers() =>
            Container.Bind<GameTriggers>().AsSingle();

        
        private void RegisterInputManager() =>
            Container.Bind<InputManager>().AsSingle();

    }
}