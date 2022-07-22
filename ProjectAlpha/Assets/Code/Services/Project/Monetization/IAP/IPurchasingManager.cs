using UnityEngine.Purchasing;

namespace Code.Services.Monetization;

public interface IPurchasingManager
{
    void PurchaseComplete(Product product);
    void PurchaseFailed(Product product, PurchaseFailureReason failureReason);
}
