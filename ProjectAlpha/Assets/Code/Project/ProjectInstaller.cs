using Code.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Project
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [FormerlySerializedAs("camera")] 
        [Required, SerializeField]
        private Camera mainCamera;
        
        [Required, SerializeField]
        private SceneReferences sceneReferences;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitUniTaskLoop()
        {
            //var loop = PlayerLoop.GetCurrentPlayerLoop();
            // // minimum is Update | FixedUpdate | LastPostLateUpdate
            //PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
        }
        
        private void Awake()
        {
            DOTween.Init();
        }

        public override void InstallBindings()
        {
            Debug.Log("ProjectInstaller.InstallBindings" + ": " + Time.frameCount);
            //how many frames it takes to load a GAME scene

            RegisterCamera();
            Container.BindInstance(sceneReferences);
            
            Container.Bind<SceneLoader>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ScreenSizeChecker>().AsSingle();
        }

        private void RegisterCamera()
        {
            Camera cam = Instantiate(mainCamera);
            DontDestroyOnLoad(cam); //exception on editor zenject validation 
            Container.BindInstance(cam);
        }
    }
}