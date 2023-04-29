using Code.Services.Data;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Code.Services.Monetization
{
    public class IAPManager : IIAPManager
    {
        private readonly IProgress _progress;
        private readonly Settings _settings;

        public IAPManager(IProgress progress, Settings settings)
        {
            _progress = progress;
            _settings = settings;
        }

        public void PurchaseComplete(Product product)
        {
            if (product.definition.payout.type == _settings.CherryPayoutType)
            {
                int cherries = (int)product.definition.payout.quantity;
                _progress.Persistant.AddCherries(cherries);
            }

            if (product.definition.id == _settings.NoAdsId)
            {
                _progress.Persistant.DisableAds();
            }
        }

        public void PurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            if (failureDescription.reason == PurchaseFailureReason.UserCancelled) return;
            Debug.Log($"Purchase failed - productId: '{product.definition.id}', reason: '{failureDescription.reason}', message: '{failureDescription.message}'");
        }

        [System.Serializable]
        public class Settings
        {
            public PayoutType CherryPayoutType = PayoutType.Currency;
            public string NoAdsId = "com.code.disable_ads";
        }
    }
}