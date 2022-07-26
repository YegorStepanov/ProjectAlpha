﻿using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuMediator _menuMediator;
    private MainMenu _mainMenu;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadMediator = loader.LoadAssetAsync(MenuAddress.MenuMediator);
        var loadMainMenu = loader.LoadAssetAsync(MenuAddress.MainMenu);

        (_menuMediator, _mainMenu) = await (loadMediator, loadMainMenu);
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_menuMediator, Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_mainMenu, Lifetime.Singleton);
        builder.InjectGameObject<MainMenu>();

        builder.Register<PanelManager>(Lifetime.Singleton);
    }
}
