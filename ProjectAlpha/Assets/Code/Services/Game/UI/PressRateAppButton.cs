using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class PressRateAppButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private GameMediator _gameMediator;

    public void OnPointerClick(PointerEventData eventData)
    {
        Assert.IsTrue(_gameMediator != null);
    }
}