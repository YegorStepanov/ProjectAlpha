using Code.AddressableAssets;
using Code.Services;
using UnityEngine;

namespace Code;

public static class MenuAddress
{
    //rename panels to canvases or smth that?
    public static readonly Address<GameObject> ShopPanel = new("Shop Panel");
    public static readonly Address<GameObject> HeroSelectorPanel = new("Hero Selector Panel");

    public static readonly Address<MenuMediator> MenuMediator = new("Menu Mediator");
    public static readonly Address<MainMenu> MainMenu = new("Main Menu");
}
