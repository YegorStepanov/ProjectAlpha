using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuMediator _menu;
    private MainMenu _mainMenu;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadMediator = loader.LoadAssetAsync(MenuAddress.MenuMediator);
        var loadMainMenu = loader.LoadAssetAsync(MenuAddress.MainMenu);

        (_menu, _mainMenu) = await (loadMediator, loadMainMenu);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_menu, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_mainMenu, Lifetime.Singleton);

        builder.Register<PanelManager>(Lifetime.Singleton);

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        builder.RegisterBuildCallback(BuildCallback);
    }

    private static void BuildCallback(IObjectResolver resolver)
    {
        var mainMenu = resolver.Resolve<MainMenu>();
        resolver.InjectGameObject(mainMenu.gameObject);
    }
}