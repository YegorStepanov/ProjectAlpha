using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI;

public sealed class StartGameTrigger : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IPublisher<Event.GameStart> _gameStartEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameStartEvent.Publish(new());
    }
}
