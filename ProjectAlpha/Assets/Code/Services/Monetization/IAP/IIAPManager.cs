using UnityEngine.Purchasing;

namespace Code.Services.Monetization.IAP;

public interface IIAPManager
{
    void PurchaseComplete(Product product);
    void PurchaseFailed(Product product, PurchaseFailureReason failureReason);
}
