using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Code.Menu
{
    public sealed class ShowShopPanel : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private MenuMediator menu;

        public void OnPointerClick(PointerEventData eventData) =>
            menu.ShowShopPanel();
    }
}