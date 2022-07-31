using Code.Services.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class PressLeaderboardButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IGameUIActions _gameUIActions;

    public void OnPointerClick(PointerEventData eventData)
    {
        Assert.IsTrue(_gameUIActions != null);
    }
}
