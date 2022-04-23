﻿using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Menu
{
    public sealed class HideHeroSelectorPanel : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private MenuMediator menu;
    
        public void OnPointerClick(PointerEventData eventData) => menu.HideHeroSelectorPanel();
    }
}