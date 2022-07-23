using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.Services.Monetization;

public class IAPManager : IIAPManager
{
    private readonly PlayerProgress _playerProgress;
    private readonly Settings _settings;

    public IAPManager(PlayerProgress playerProgress, Settings settings)
    {
        _playerProgress = playerProgress;
        _settings = settings;
    }

    public void PurchaseComplete(Product product)
    {
        if (product.definition.payout.type == _settings.CherryPayoutType)
        {
            int cherries = (int)product.definition.payout.quantity;
            _playerProgress.AddCherries(cherries);
        }

        if (product.definition.id == _settings.NoAdsId)
        {
            _playerProgress.DisableAds();
        }
    }

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        if (failureReason == PurchaseFailureReason.UserCancelled) return;
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    [System.Serializable]
    public class Settings
    {
        public PayoutType CherryPayoutType = PayoutType.Currency;
        public string NoAdsId = "com.code.disable_ads";
    }
}
