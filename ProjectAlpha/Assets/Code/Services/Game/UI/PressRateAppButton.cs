﻿using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class PressRateAppButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IGameUIMediator _gameUIMediator;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameUIMediator.RequestStoreReview();
    }
}
