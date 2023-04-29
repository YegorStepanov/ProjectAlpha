using Code.Scopes;
using Code.Services.UI;
using Sirenix.OdinInspector;

namespace Code.Services.Development
{
    public sealed class DevelopmentMenuPanel : DevelopmentPanelPart<MenuScope>
    {
        private readonly IMenuUIFacade _menuUIFacade;
        private readonly PanelManager _panelManager;

        public DevelopmentMenuPanel(IMenuUIFacade menuUIFacade, PanelManager panelManager, DevelopmentPanel panel) :
            base(panel)
        {
            _menuUIFacade = menuUIFacade;
            _panelManager = panelManager;
        }

        [Button] private void CloseScene() => _menuUIFacade.CloseScene();
        [Button] private void ToggleSound() => _menuUIFacade.ToggleSound();
        [Button] private void EnableAds() => _menuUIFacade.EnableAds();
        [Button] private void DisableAds() => _menuUIFacade.DisableAds();
        [Button] private void ShowRewardedAd() => _menuUIFacade.ShowRewardedAd();

#if UNITY_EDITOR
        [BoxGroup("Shop")]
        [HorizontalGroup("Shop/a"), Button("Show")]
        private void ShowShopPanel() => _menuUIFacade.Open<ShopPanel>();

        [HorizontalGroup("Shop/a"), Button("Hide")]
        private void HideShopPanel() => _menuUIFacade.Close<ShopPanel>();

        [HorizontalGroup("Shop/a"), Button("Unload")]
        private void UnloadShopPanel() => _panelManager.Unload<ShopPanel>();

        [BoxGroup("Hero")]
        [HorizontalGroup("Hero/a"), Button("Show")]
        private void ShowHeroSelectorPanel() => _menuUIFacade.Open<HeroSelectorPanel>();

        [HorizontalGroup("Hero/a"), Button("Hide")]
        private void HideHeroSelectorPanel() => _menuUIFacade.Close<HeroSelectorPanel>();

        [HorizontalGroup("Hero/a"), Button("Unload")]
        private void UnloadHeroSelectorPanel() => _panelManager.Unload<HeroSelectorPanel>();
#endif
    }
}