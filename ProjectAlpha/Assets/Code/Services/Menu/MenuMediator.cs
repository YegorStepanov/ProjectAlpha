using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class MenuMediator : MonoBehaviour
{
    [Inject] private UIManager _uiManager;

    public void Open<TPanel>() where TPanel : struct, IPanel =>
        _uiManager.Show<TPanel>();

    public void Close<TPanel>() where TPanel : struct, IPanel =>
        _uiManager.Hide<TPanel>();

    public void CloseMainMenu() => _uiManager.Unload<MainMenu>();

    //Odin doesn't recognize generic methods, so resolve them manually
    
    [BoxGroup("Main Menu")]
    [HorizontalGroup("Main Menu/b")] [Button("Show")]
    private void ShowMainMenu() => _uiManager.Show<MainMenu>();

    [HorizontalGroup("Main Menu/b")] [Button("Hide")]
    private void HideMainMenu() => _uiManager.Hide<MainMenu>();

    [HorizontalGroup("Main Menu/b")] [Button("Unload")]
    private void UnloadMainMenu() => _uiManager.Unload<MainMenu>();

    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b")] [Button("Show")]
    private void ShowShopPanel() => _uiManager.Show<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")] [Button("Hide")]
    private void HideShopPanel() => _uiManager.Hide<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")] [Button("Unload")]
    private void UnloadShopPanel() => _uiManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b")] [Button("Show")]
    private void ShowHeroSelectorPanel() => _uiManager.Show<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")] [Button("Hide")]
    private void HideHeroSelectorPanel() => _uiManager.Hide<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")] [Button("Unload")]
    private void UnloadHeroSelectorPanel() => _uiManager.Unload<HeroSelectorPanel>();
}