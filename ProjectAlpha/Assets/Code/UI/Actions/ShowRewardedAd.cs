﻿using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class ShowRewardedAd : MonoBehaviour, IPointerClickHandler
{
    [Inject] private MenuMediator _menu;

    public void OnPointerClick(PointerEventData eventData) =>
        _menu.ShowRewardedAd();
}