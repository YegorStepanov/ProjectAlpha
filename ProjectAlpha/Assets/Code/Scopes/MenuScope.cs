using Code.AddressableAssets;
using Code.Common;
using Code.Extensions;
using Code.Services.Development;
using Code.Services.Navigators;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private IMenuUIFacade _menuUIFacade;
    private IMainMenuView _mainMenuView;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        (_menuUIFacade, _mainMenuView) = await (
            loader.InstantiateAsync(Address.UI.MenuUIActions, inject: false),
            loader.InstantiateAsync(Address.UI.MainMenuView, inject: false));
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.RegisterComponentAndInjectGameObject(_menuUIFacade);
        builder.RegisterComponentAndInjectGameObject(_mainMenuView);

        builder.Register<PanelManager>(Lifetime.Singleton);
        RegisterDevelopment(builder);
        RegisterNavigator(builder);
    }

    private static void RegisterDevelopment(IContainerBuilder builder)
    {
        if (PlatformInfo.IsDevelopment)
            builder.RegisterNonLazy<DevelopmentMenuPanel>(Lifetime.Singleton);
    }

    private static void RegisterNavigator(IContainerBuilder builder)
    {
        builder.Register<IMenuSceneNavigator, MenuSceneNavigator>(Lifetime.Singleton);
    }
}
