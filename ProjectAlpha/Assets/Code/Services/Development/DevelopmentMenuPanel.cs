using System;
using Code.Services.UI;
using Sirenix.OdinInspector;

namespace Code.Services.Development;

public sealed class DevelopmentMenuPanel : IDisposable
{
    private readonly IMenuUIActions _menuUIActions;
    private readonly PanelManager _panelManager;
    private readonly DevelopmentPanel _panel;

    public DevelopmentMenuPanel(DevelopmentPanel panel, IMenuUIActions menuUIActions, PanelManager panelManager)
    {
        _menuUIActions = menuUIActions;
        _panelManager = panelManager;
        _panel = panel;
        _panel.Bind(this);
    }

    public void Dispose() =>
        _panel.Unbind(this);

    [Button] public void CloseScene() => _menuUIActions.CloseScene();
    [Button] public void ToggleSound() => _menuUIActions.ToggleSound();
    [Button] public void EnableAds() => _menuUIActions.EnableAds();
    [Button] public void DisableAds() => _menuUIActions.DisableAds();
    [Button] public void ShowRewardedAd() => _menuUIActions.ShowRewardedAd();

#if UNITY_EDITOR
    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b")]
    [Button("Show"), HideIf("_menuUIActions", null)]
    private void ShowShopPanel() => _menuUIActions.Open<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Hide"), HideIf("_menuUIActions", null)]
    private void HideShopPanel() => _menuUIActions.Close<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Unload"), HideIf("_panelManager", null)]
    private void UnloadShopPanel() => _panelManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Show"), HideIf("_menuUIActions", null)]
    private void ShowHeroSelectorPanel() => _menuUIActions.Open<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Hide"), HideIf("_menuUIActions", null)]
    private void HideHeroSelectorPanel() => _menuUIActions.Close<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Unload"), HideIf("_panelManager", null)]
    private void UnloadHeroSelectorPanel() => _panelManager.Unload<HeroSelectorPanel>();
#endif
}
