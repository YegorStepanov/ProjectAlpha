using UnityEngine.Purchasing;

namespace Code.Services.UI;

public interface IMenuUIFacade
{
    void Open<TPanel>() where TPanel : struct, IPanel;
    void Close<TPanel>() where TPanel : struct, IPanel;

    void CloseScene();
    void ToggleSound();
    void EnableAds();
    void DisableAds();
    void ShowRewardedAd();
    void RaiseGameStartEvent();

    void PurchaseComplete(Product product);
    void PurchaseFailed(Product product, PurchaseFailureReason failureReason);
}
