using Code.Services.Data;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using MessagePipe;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using VContainer;
using Event = Code.Common.Event;

namespace Code.Services.UI
{
    public sealed class MenuUIFacade : MonoBehaviour, IMenuUIFacade
    {
        [Inject] private IMainMenuView _mainMenuView;
        [Inject] private PanelManager _panelManager;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IIAPManager _iapManager;
        [Inject] private IProgress _progress;
        [Inject] private AdsManager _adsManager;
        [Inject] private IPublisher<Event.GameStart> _gameStartEvent;

        public void Open<TPanel>() where TPanel : struct, IPanel => _panelManager.Show<TPanel>();
        public void Close<TPanel>() where TPanel : struct, IPanel => _panelManager.Hide<TPanel>();

        public void CloseScene() => _sceneLoader.UnloadAsync<MenuScene>();
        public void ToggleSound() => _mainMenuView.ToggleSound();
        public void EnableAds() => _progress.Persistant.EnableAds();
        public void DisableAds() => _progress.Persistant.DisableAds();
        public void ShowRewardedAd() => _adsManager.ShowRewardedAd();
        public void RaiseGameStartEvent() => _gameStartEvent.Publish(new Event.GameStart());

        public void PurchaseComplete(Product product) =>
            _iapManager.PurchaseComplete(product);

        public void PurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            _iapManager.PurchaseFailed(product, failureDescription);
    }
}