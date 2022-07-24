using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class PressRateAppButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private GameMediator _gameMediator;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameMediator.RequestStoreReview();
    }
}
