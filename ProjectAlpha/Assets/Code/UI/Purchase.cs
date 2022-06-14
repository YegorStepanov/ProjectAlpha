using Code.Services;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Purchasing;
using VContainer;

namespace Code.UI;

public sealed class Purchase : MonoBehaviour
{
    [Inject, UsedImplicitly] private MenuMediator _menu;

    public void PurchaseComplete(Product product) =>
        _menu.PurchaseComplete(product);

    public void PurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
        _menu.PurchaseFailed(product, failureReason);
}