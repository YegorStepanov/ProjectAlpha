using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.Services.Monetization;

public class PurchasingManager : IPurchasingManager
{
    private const string NoAdsData = "no_ads";

    private readonly PlayerProgress _playerProgress;

    public PurchasingManager(PlayerProgress playerProgress)
    {
        _playerProgress = playerProgress;
    }

    public void PurchaseComplete(Product product)
    {
        if (product.definition.payout.type == PayoutType.Currency)
        {
            int cherryCount = (int)product.definition.payout.quantity;
            _playerProgress.AddCherryCount(cherryCount);
        }

        if (product.definition.payout.data == NoAdsData)
        {
            _playerProgress.SetNoAds();
        }
    }

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
}