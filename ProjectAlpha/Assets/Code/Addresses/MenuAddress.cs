using UnityEngine;

namespace Code;

public static class MenuAddress
{
    //rename panels to canvases or smth that?
    public static readonly Address<GameObject> MainMenu = new("Main Menu");
    public static readonly Address<GameObject> ShopPanel = new("Shop Panel");
    public static readonly Address<GameObject> HeroSelectorPanel = new("Hero Selector Panel");

    //Loading Screen?
}