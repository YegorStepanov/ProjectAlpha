﻿using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Actions
{
    public sealed class CloseHeroSelectorPanel : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private MenuMediator menu;

        public void OnPointerClick(PointerEventData eventData) =>
            menu.Close<HeroSelectorPanel>();
    }
}