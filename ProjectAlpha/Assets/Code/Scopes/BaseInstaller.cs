using Code.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Scopes;

public class BaseInstaller<TInitializer> : MonoInstaller
    where TInitializer : class, IInitializable
{
    public override void InstallBindings()
    {
        RegisterCancellationToken();

        RegisterAddressableFactory();

        RegisterInitializer();
    }

    private void RegisterCancellationToken() =>
        Container.BindInstance(this.GetCancellationTokenOnDestroy());

    private void RegisterAddressableFactory() =>
        Container.Bind<AddressableFactory>().AsSingle().WithArguments(transform);

    private void RegisterInitializer() =>
        Container.BindInterfacesTo<TInitializer>().AsSingle();
}