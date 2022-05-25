using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuMediator _menu;
    private MainMenuController _mainMenu;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        _menu = await loader.LoadAssetAsync(MenuAddress.MenuMediator);
        _mainMenu = await loader.LoadAssetAsync(MenuAddress.MainMenu);
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
        var mainMenu = resolver.Resolve<MainMenuController>();
        resolver.InjectGameObject(mainMenu.gameObject);
    }
}