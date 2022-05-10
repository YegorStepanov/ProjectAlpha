using Code.Services;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapScope : LifetimeScope
{
    public LoadingScreen _loadingScreen;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_loadingScreen);

        builder.RegisterEntryPoint<BootstrapStart>();
        
        builder.Register<AddressableFactory>(Lifetime.Singleton); //todo:
    }
}