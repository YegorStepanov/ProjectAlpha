using Sirenix.OdinInspector;
using UnityEngine;
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
        private Scenes scenes;

        public override void InstallBindings()
        {
            GameObject.Instantiate(mainCamera);

            Debug.Log("ProjectInstaller.InstallBindings");
            //how many frames it takes to load a GAME scene

            Container.BindInstance(mainCamera);
            Container.BindInstance(scenes);
            
            Container.Bind<SceneLoader>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ScreenSizeChecker>().AsSingle();

            Container.BindInterfacesTo<ProjectInitializer>().AsSingle();
        }
    }
}