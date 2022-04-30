using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Actions;

public sealed class OpenShopPanel : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MenuMediator menu;

    public void OnPointerClick(PointerEventData eventData) =>
        menu.Open<ShopPanel>();
}