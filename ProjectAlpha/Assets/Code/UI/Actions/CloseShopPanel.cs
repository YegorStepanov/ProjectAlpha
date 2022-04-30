using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Actions;

public sealed class CloseShopPanel : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MenuMediator menu;

    public void OnPointerClick(PointerEventData eventData) =>
        menu.Close<ShopPanel>();
}