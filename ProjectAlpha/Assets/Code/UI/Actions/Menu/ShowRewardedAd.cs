using Code.Services.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions.Menu;

public sealed class ShowRewardedAd : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MenuUIActions _menu;

    public void OnPointerClick(PointerEventData eventData) =>
        _menu.ShowRewardedAd();
}
