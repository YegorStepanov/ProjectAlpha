using System.Threading;
using Code.Services.Monetization;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.Services;

public sealed class MenuMediator : MonoBehaviour
{
    private MainMenu _mainMenu;
    private PanelManager _panelManager;
    private ISceneLoader _sceneLoader;
    private IPurchasingManager _purchasingManager;
    private PlayerProgress _playerProgress;
    private AdsManager _adsManager;
    private CancellationToken _token;

    [Inject, UsedImplicitly]
    public void Construct(MainMenu mainMenu, PanelManager panelManager, ISceneLoader sceneLoader, IPurchasingManager purchasingManager, PlayerProgress playerProgress, AdsManager adsManager, CancellationToken token)
    {
        _adsManager = adsManager;
        _mainMenu = mainMenu;
        _panelManager = panelManager;
        _sceneLoader = sceneLoader;
        _purchasingManager = purchasingManager;
        _playerProgress = playerProgress;
        _token = token;
    }

    public void Open<TPanel>() where TPanel : struct, IPanel => _panelManager.Show<TPanel>();
    public void Close<TPanel>() where TPanel : struct, IPanel => _panelManager.Hide<TPanel>();

    [Button] public void CloseScene() => _sceneLoader.UnloadAsync<MenuScene>(_token);
    [Button] public void ToggleSound() => _mainMenu.ToggleSound();
    [Button] public void EnableAds() => _playerProgress.EnableAds();
    [Button] public void DisableAds() => _playerProgress.DisableAds();
    [Button] public void WatchRewardedAd() => _adsManager.WatchRewardedAd();

    public void PurchaseComplete(Product product) =>
        _purchasingManager.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _purchasingManager.PurchaseFailed(product, failureReason);

#if UNITY_EDITOR
    //Odin doesn't recognize generic methods, so resolve them manually
    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b"), Button("Show")]
    private void ShowShopPanel() => _panelManager.Show<ShopPanel>();

    [HorizontalGroup("Shop Panel/b"), Button("Hide")]
    private void HideShopPanel() => _panelManager.Hide<ShopPanel>();

    [HorizontalGroup("Shop Panel/b"), Button("Unload")]
    private void UnloadShopPanel() => _panelManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b"), Button("Show")]
    private void ShowHeroSelectorPanel() => _panelManager.Show<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b"), Button("Hide")]
    private void HideHeroSelectorPanel() => _panelManager.Hide<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b"), Button("Unload")]
    private void UnloadHeroSelectorPanel() => _panelManager.Unload<HeroSelectorPanel>();
#endif
}
