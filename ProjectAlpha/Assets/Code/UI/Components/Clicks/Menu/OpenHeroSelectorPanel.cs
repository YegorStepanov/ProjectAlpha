﻿using Code.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Components
{
    public sealed class OpenHeroSelectorPanel : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private IMenuUIFacade _menu;

        public void OnPointerClick(PointerEventData eventData) =>
            _menu.Open<HeroSelectorPanel>();
    }
}
