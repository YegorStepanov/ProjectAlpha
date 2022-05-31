using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class PressLeaderboardButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private GameUIMediator _gameUIMediator;

    public void OnPointerClick(PointerEventData eventData)
    {
        Assert.IsTrue(_gameUIMediator != null);
    }
}