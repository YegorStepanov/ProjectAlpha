using Code.AddressableAssets.Loaders;
using Code.Extensions;
using Code.Services.UI;
using Code.Services.UI.Menu;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuUIActions _menuUIActions;
    private MainMenuView _mainMenuView;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadMediator = loader.LoadAssetAsync(Address.UI.MenuMediator);
        var loadMainMenu = loader.LoadAssetAsync(Address.UI.MainMenu);

        (_menuUIActions, _mainMenuView) = await (loadMediator, loadMainMenu);
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_menuUIActions, Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_mainMenuView, Lifetime.Singleton);
        builder.InjectGameObject<MainMenuView>();

        builder.Register<PanelManager>(Lifetime.Singleton);
    }
}
