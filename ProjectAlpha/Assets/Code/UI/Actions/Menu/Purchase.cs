using Code.Services.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.UI.Actions;

public sealed class Purchase : MonoBehaviour
{
    [Inject] private IMenuUIActions _menu;

    public void PurchaseComplete(Product product) =>
        _menu.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _menu.PurchaseFailed(product, failureReason);
}
