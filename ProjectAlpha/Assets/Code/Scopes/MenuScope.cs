using Code.AddressableAssets;
using Code.Common;
using Code.Extensions;
using Code.Services.Development;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private IMenuUIActions _menuUIActions;
    private IMainMenuView _mainMenuView;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        (_menuUIActions, _mainMenuView) = await (
            loader.InstantiateAsync(Address.UI.MenuUIActions, inject: false),
            loader.InstantiateAsync(Address.UI.MainMenuView, inject: false));
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.RegisterComponentAndInjectGameObject(_menuUIActions);
        builder.RegisterComponentAndInjectGameObject(_mainMenuView);

        builder.Register<PanelManager>(Lifetime.Singleton);
        RegisterDevelopment(builder);
    }

    private static void RegisterDevelopment(IContainerBuilder builder)
    {
        if(PlatformInfo.IsDevelopment)
            builder.RegisterNonLazy<DevelopmentMenuPanel>(Lifetime.Singleton);
    }
}
