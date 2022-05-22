using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuMediator _menu;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        _menu = await loader.InstantiateAsync(MenuAddress.MenuMediator);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_menu);
        builder.Register<UIManager>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MenuStart>();

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
    }
}