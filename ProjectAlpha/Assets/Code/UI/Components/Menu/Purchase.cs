using Code.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using VContainer;

namespace Code.UI.Components
{
    public sealed class Purchase : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _count;

        [Inject] private IMenuUIFacade _menu;

        public void PurchaseComplete(Product product) =>
            _menu.PurchaseComplete(product);

        public void PurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            _menu.PurchaseFailed(product, failureDescription);

        public void PurchaseFetched(Product product)
        {
            _price.text = $"{product.metadata.localizedPriceString} {product.metadata.isoCurrencyCode}";
            _count.text = product.metadata.localizedDescription;
        }
    }
}
