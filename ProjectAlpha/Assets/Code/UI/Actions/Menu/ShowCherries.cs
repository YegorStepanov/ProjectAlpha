﻿using Code.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class ShowCherries : MonoBehaviour, IPointerClickHandler
{
    [Inject] private IMenuUIActions _menu;

    public void OnPointerClick(PointerEventData eventData) =>
        _menu.CloseScene();
}
