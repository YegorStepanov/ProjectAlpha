using Code.Services;
using Cysharp.Threading.Tasks;
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
    }
}