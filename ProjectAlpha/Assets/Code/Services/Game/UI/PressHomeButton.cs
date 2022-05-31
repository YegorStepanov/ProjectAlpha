﻿using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class PressHomeButton : MonoBehaviour, IPointerClickHandler
{
    [Inject] private GameUIMediator _gameUIMediator;

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameUIMediator.LoadMenu();
    }
}