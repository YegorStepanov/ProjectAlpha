using Code.Services.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.UI.Components;

public sealed class Purchase : MonoBehaviour
{
    [Inject] private IMenuUIFacade _menu;

    public void PurchaseComplete(Product product) =>
        _menu.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _menu.PurchaseFailed(product, failureReason);
}
