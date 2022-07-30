using Code.AddressableAssets.Loaders;
using Code.Scopes.EntryPoints;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapScope : Scope
{
    private LoadingScreen _loadingScreen;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        _loadingScreen = await loader.LoadAssetAsync(Address.UI.LoadingScreen);
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_loadingScreen, Lifetime.Singleton);

        builder.RegisterEntryPoint<BootstrapEntryPoint>();
    }
}
