using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using Event = Code.Common.Event;

namespace Code.UI.Actions.Menu;

public sealed class RaiseGameStartEvent : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IPublisher<Event.GameStart> _gameStartEvent; //todo

    public void OnPointerClick(PointerEventData eventData) =>
        _gameStartEvent.Publish(new());
}
