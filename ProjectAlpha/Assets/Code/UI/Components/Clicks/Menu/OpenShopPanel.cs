using Code.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Components;

public sealed class OpenShopPanel : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IMenuUIFacade _menu;

    public void OnPointerClick(PointerEventData eventData) =>
        _menu.Open<ShopPanel>();
}
