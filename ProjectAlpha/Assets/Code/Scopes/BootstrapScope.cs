using Code.AddressableAssets;
using Code.Services.Navigators;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes
{
    public sealed class BootstrapScope : Scope
    {
        [SerializeField] private LoadingScreen _bootLoadingScreen;

        protected override UniTask PreloadAsync(IAddressablesLoader loader) =>
            UniTask.CompletedTask;

        protected override void ConfigureServices(IContainerBuilder builder)
        {
            builder.RegisterInstance<ILoadingScreen>(_bootLoadingScreen);

            builder.Register<IBootstrapSceneNavigator, BootstrapSceneNavigator>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootstrapEntryPoint>();
        }
    }
}