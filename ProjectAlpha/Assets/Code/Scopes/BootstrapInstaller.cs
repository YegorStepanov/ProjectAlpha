using Code.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Scopes
{
    public sealed class BootstrapInstaller : MonoInstaller //: BaseInstaller<BootstrapInitializer>
    {
        public LoadingScreen loadingScreen;

        public override void InstallBindings()
        {
            //base.InstallBindings();

            RegisterLoadingScreen();


            RegisterCancellationToken();

            RegisterAddressableFactory();

            RegisterInitializer();
        }

        private void RegisterCancellationToken() =>
            Container.BindInstance(this.GetCancellationTokenOnDestroy());

        private void RegisterAddressableFactory() =>
            Container.Bind<AddressableFactory>().AsSingle().WithArguments(transform);

        private void RegisterInitializer() =>
            Container.BindInterfacesTo<BootstrapInitializer>().AsSingle();


        private void RegisterLoadingScreen() =>
            Container.BindInstance(loadingScreen);
    }
}