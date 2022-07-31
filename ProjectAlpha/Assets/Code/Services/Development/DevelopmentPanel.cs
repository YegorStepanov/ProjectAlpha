using Code.Services.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Code.Services.Development;

public sealed class DevelopmentPanel : MonoBehaviour
{
    [Inject] private IObjectResolver _resolver;
    private IMenuUIActions MenuUIActions => _resolver.Resolve<IMenuUIActions>();
    private PanelManager PanelManager => _resolver.Resolve<PanelManager>();

    [Button] public void CloseScene() => MenuUIActions.CloseScene();
    [Button] public void ToggleSound() => MenuUIActions.ToggleSound();
    [Button] public void EnableAds() => MenuUIActions.EnableAds();
    [Button] public void DisableAds() => MenuUIActions.DisableAds();
    [Button] public void ShowRewardedAd() => MenuUIActions.ShowRewardedAd();

#if UNITY_EDITOR
    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b")]
    [Button("Show"), EnableIf("$PanelManager != null")]
    private void ShowShopPanel() => MenuUIActions.Open<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Hide"), EnableIf("$PanelManager != null")]
    private void HideShopPanel() => MenuUIActions.Close<ShopPanel>();

    [HorizontalGroup("Shop Panel/b")]
    [Button("Unload"), EnableIf("$PanelManager != null")]
    private void UnloadShopPanel() => PanelManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Show"), EnableIf("$PanelManager != null")]
    private void ShowHeroSelectorPanel() => MenuUIActions.Open<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Hide"), EnableIf("$PanelManager != null")]
    private void HideHeroSelectorPanel() => MenuUIActions.Close<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b")]
    [Button("Unload"), EnableIf("$PanelManager != null")]
    private void UnloadHeroSelectorPanel() => PanelManager.Unload<HeroSelectorPanel>();
#endif
}
