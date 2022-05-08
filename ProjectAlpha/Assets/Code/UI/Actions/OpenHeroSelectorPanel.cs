using Code.Services;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Code.UI.Actions;

public sealed class OpenHeroSelectorPanel : MonoBehaviour, IPointerClickHandler
{
    [Inject, UsedImplicitly] private MenuMediator _menu;

    public void OnPointerClick(PointerEventData eventData) =>
        _menu.Open<HeroSelectorPanel>();
}