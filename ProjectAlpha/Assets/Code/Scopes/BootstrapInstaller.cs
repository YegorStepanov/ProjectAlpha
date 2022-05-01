using Code.Services;

namespace Code.Scopes;

public sealed class BootstrapInstaller : BaseInstaller<BootstrapInitializer>
{
    public LoadingScreen _loadingScreen;

    public override void InstallBindings()
    {
        base.InstallBindings();

        RegisterLoadingScreen();
    }

    private void RegisterLoadingScreen() =>
        Container.BindInstance(_loadingScreen);
}