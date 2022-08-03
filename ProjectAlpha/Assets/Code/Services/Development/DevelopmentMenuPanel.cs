using System;
using Code.Services.UI;
using Sirenix.OdinInspector;

namespace Code.Services.Development;

public sealed class DevelopmentMenuPanel : IDisposable
{
    private readonly IMenuUIFacade _menuUIFacade;
    private readonly PanelManager _panelManager;
    private readonly DevelopmentPanel _panel;

    public DevelopmentMenuPanel(DevelopmentPanel panel, IMenuUIFacade menuUIFacade, PanelManager panelManager)
    {
        _menuUIFacade = menuUIFacade;
        _panelManager = panelManager;
        _panel = panel;
        _panel.Bind(this);
    }

    public void Dispose() =>
        _panel.Unbind(this);

    [Button] public void CloseScene() => _menuUIFacade.CloseScene();
    [Button] public void ToggleSound() => _menuUIFacade.ToggleSound();
    [Button] public void EnableAds() => _menuUIFacade.EnableAds();
    [Button] public void DisableAds() => _menuUIFacade.DisableAds();
    [Button] public void ShowRewardedAd() => _menuUIFacade.ShowRewardedAd();

#if UNITY_EDITOR
    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b")]
    [Button("Show"), HideIf("_menuUIActions", null)]
    private void ShowShopPanel() => _menuUIFacade.Open<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Hide"), HideIf("_menuUIActions", null)]
    private void HideShopPanel() => _menuUIFacade.Close<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Unload"), HideIf("_panelManager", null)]
    private void UnloadShopPanel() => _panelManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Show"), HideIf("_menuUIActions", null)]
    private void ShowHeroSelectorPanel() => _menuUIFacade.Open<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Hide"), HideIf("_menuUIActions", null)]
    private void HideHeroSelectorPanel() => _menuUIFacade.Close<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Unload"), HideIf("_panelManager", null)]
    private void UnloadHeroSelectorPanel() => _panelManager.Unload<HeroSelectorPanel>();
#endif
}
