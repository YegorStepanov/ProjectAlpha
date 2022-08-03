using Code.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Components;

public sealed class PressLeaderboardButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IGameUIFacade _gameUIFacade;

    public void OnPointerClick(PointerEventData eventData) { }
}
