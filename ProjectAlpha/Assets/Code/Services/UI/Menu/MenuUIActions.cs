using System.Threading;
using Code.Services.Data;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using Code.Services.Monetization.IAP;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.Services.UI.Menu;

public sealed class MenuUIActions : MonoBehaviour
{
    private MainMenuView _mainMenuView;
    private PanelManager _panelManager;
    private ISceneLoader _sceneLoader;
    private IIAPManager _iapManager;
    private IProgress _progress;
    private AdsManager _adsManager;
    private CancellationToken _token;

    [Inject]
    public void Construct(MainMenuView mainMenuView, PanelManager panelManager, ISceneLoader sceneLoader, IIAPManager iapManager, IProgress progress, AdsManager adsManager, CancellationToken token)
    {
        _progress = progress;
        _adsManager = adsManager;
        _mainMenuView = mainMenuView;
        _panelManager = panelManager;
        _sceneLoader = sceneLoader;
        _iapManager = iapManager;
        _token = token;
    }

    public void Open<TPanel>() where TPanel : struct, IPanel => _panelManager.Show<TPanel>();
    public void Close<TPanel>() where TPanel : struct, IPanel => _panelManager.Hide<TPanel>();

    public void CloseScene() => _sceneLoader.UnloadAsync<MenuScene>(_token);
    public void ToggleSound() => _mainMenuView.ToggleSound();
    public void EnableAds() => _progress.Persistant.EnableAds();
    public void DisableAds() => _progress.Persistant.DisableAds();
    public void ShowRewardedAd() => _adsManager.ShowRewardedAd();

    public void PurchaseComplete(Product product) =>
        _iapManager.PurchaseComplete(product);
    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _iapManager.PurchaseFailed(product, failureReason);
}
