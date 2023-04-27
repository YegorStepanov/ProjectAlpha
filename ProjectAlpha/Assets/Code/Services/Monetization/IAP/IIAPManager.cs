using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Code.Services.Monetization
{
    public interface IIAPManager
    {
        void PurchaseComplete(Product product);
        void PurchaseFailed(Product product, PurchaseFailureDescription failureDescription);
    }
}
