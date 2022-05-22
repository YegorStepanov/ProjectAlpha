using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapScope : Scope
{
    private LoadingScreen _loadingScreen;
    
    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        _loadingScreen = await loader.LoadAssetAsync(BootstrapAddress.LoadingScreen);
    }
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(this.InstantiateInScene(_loadingScreen));

        builder.RegisterEntryPoint<BootstrapStart>();
    }
}