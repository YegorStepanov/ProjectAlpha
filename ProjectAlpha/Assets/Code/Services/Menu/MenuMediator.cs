﻿using System.Threading;
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
    private IIAPManager _iapManager;
    private IProgress _progress;
    private AdsManager _adsManager;
    private CancellationToken _token;

    [Inject, UsedImplicitly]
    public void Construct(MainMenu mainMenu, PanelManager panelManager, ISceneLoader sceneLoader, IIAPManager iapManager, IProgress progress, AdsManager adsManager, ScopeCancellationToken token)
    {
        _progress = progress;
        _adsManager = adsManager;
        _mainMenu = mainMenu;
        _panelManager = panelManager;
        _sceneLoader = sceneLoader;
        _iapManager = iapManager;
        _token = token;
    }

    public void Open<TPanel>() where TPanel : struct, IPanel => _panelManager.Show<TPanel>();
    public void Close<TPanel>() where TPanel : struct, IPanel => _panelManager.Hide<TPanel>();

    [Button] public void CloseScene() => _sceneLoader.UnloadAsync<MenuScene>(_token);
    [Button] public void ToggleSound() => _mainMenu.ToggleSound();
    [Button] public void EnableAds() => _progress.Persistant.EnableAds();
    [Button] public void DisableAds() => _progress.Persistant.DisableAds();
    [Button] public void ShowRewardedAd() => _adsManager.ShowRewardedAd();

    public void PurchaseComplete(Product product) =>
        _iapManager.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _iapManager.PurchaseFailed(product, failureReason);

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
