using Code.AddressableAssets;
using Code.Services.UI.Menu;
using UnityEngine;

namespace Code.Addresses;

public static class MenuAddress
{
    //rename panels to canvases or smth that?
    public static readonly Address<GameObject> ShopPanel = new("Shop Panel");
    public static readonly Address<GameObject> HeroSelectorPanel = new("Hero Selector Panel");

    public static readonly Address<MenuUIActions> MenuMediator = new("Menu Mediator");
    public static readonly Address<MainMenuView> MainMenu = new("Main Menu");
}
