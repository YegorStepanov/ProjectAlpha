using Code.Services.UI.Menu;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.UI.Actions.Menu;

public sealed class Purchase : MonoBehaviour
{
    [Inject] private MenuUIActions _menu;

    public void PurchaseComplete(Product product) =>
        _menu.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _menu.PurchaseFailed(product, failureReason);
}
