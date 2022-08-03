using Code.Services.Data;
using Code.Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Code.UI.Actions;

public sealed class CherryCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _heroIndex;
    [SerializeField] private int _unlockPrice;
    [SerializeField] private Image _lockedPanel;
    [SerializeField] private TextMeshProUGUI _unlockPriceText;

    [Inject] private IProgress _progress;
    [Inject] private PanelManager _panelManager;

    private bool IsHeroLocked => _progress.Persistant.IsHeroLocked(_heroIndex);

    private void Start() =>
        UpdateLockedPanel();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsHeroLocked)
            Buy();
        else
        {
            SelectHero();
            _panelManager.Hide<HeroSelectorPanel>();
        }
    }

    private void Buy()
    {
        if (_progress.Persistant.Cherries >= _unlockPrice)
        {
            _progress.Persistant.UnlockHero(_heroIndex);
            _progress.Persistant.AddCherries(-_unlockPrice);
            UpdateLockedPanel();
        }
    }

    private void SelectHero() =>
        _progress.Persistant.SetSelectedHero(_heroIndex);

    private void UpdateLockedPanel()
    {
        _lockedPanel.gameObject.SetActive(IsHeroLocked);

        if (IsHeroLocked)
            _unlockPriceText.text = _unlockPrice.ToString();
    }
}
