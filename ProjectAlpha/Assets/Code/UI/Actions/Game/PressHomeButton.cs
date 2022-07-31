using Code.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class PressHomeButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IGameUIActions _gameUIActions;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameUIActions.LoadMenu();
    }
}
