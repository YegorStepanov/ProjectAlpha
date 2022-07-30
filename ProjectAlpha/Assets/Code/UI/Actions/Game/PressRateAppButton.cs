using Code.Services.UI.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions.Game;

public sealed class PressRateAppButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IGameUIActions _gameUIActions;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameUIActions.RequestStoreReview();
    }
}
