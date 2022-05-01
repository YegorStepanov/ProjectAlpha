using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class MenuMediator : MonoBehaviour
{
    [Inject] private UIManager uiManager;

    public void Open<TPanel>() where TPanel : struct, IPanel =>
        uiManager.Show<TPanel>();

    public void Close<TPanel>() where TPanel : struct, IPanel =>
        uiManager.Hide<TPanel>();

    public void CloseMainMenu() => uiManager.Unload<MainMenu>();

    //Odin doesn't recognize generic methods, so resolve them manually
    
    [BoxGroup("Main Menu")]
    [HorizontalGroup("Main Menu/b")] [Button("Show")]
    private void ShowMainMenu() => uiManager.Show<MainMenu>();

    [HorizontalGroup("Main Menu/b")] [Button("Hide")]
    private void HideMainMenu() => uiManager.Hide<MainMenu>();

    [HorizontalGroup("Main Menu/b")] [Button("Unload")]
    private void UnloadMainMenu() => uiManager.Unload<MainMenu>();

    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b")] [Button("Show")]
    private void ShowShopPanel() => uiManager.Show<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")] [Button("Hide")]
    private void HideShopPanel() => uiManager.Hide<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")] [Button("Unload")]
    private void UnloadShopPanel() => uiManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b")] [Button("Show")]
    private void ShowHeroSelectorPanel() => uiManager.Show<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")] [Button("Hide")]
    private void HideHeroSelectorPanel() => uiManager.Hide<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")] [Button("Unload")]
    private void UnloadHeroSelectorPanel() => uiManager.Unload<HeroSelectorPanel>();
}