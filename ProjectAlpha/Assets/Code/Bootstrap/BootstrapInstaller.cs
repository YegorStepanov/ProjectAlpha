using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Zenject;

namespace Code.Project
{
    public sealed class BootstrapInstaller : MonoInstaller
    {
        public LoadingScreen loadingScreen;

        public override void InstallBindings()
        {
            Container.BindInstance(this.GetCancellationTokenOnDestroy());
            
            Debug.Log("BootstrapInitializer.InstallBindings" + ": " + Time.frameCount);
            
            Container.BindInstance(loadingScreen);
            Container.BindInterfacesTo<BootstrapInitializer>().AsSingle();
        }
    }
}