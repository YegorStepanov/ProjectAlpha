﻿using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class ToggleSound : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MenuMediator _mainMenu;

    public void OnPointerClick(PointerEventData eventData) =>
        _mainMenu.ToggleSound();
}