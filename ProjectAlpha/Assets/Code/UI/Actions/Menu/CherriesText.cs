using Code.Services.Data;
using TMPro;
using UnityEngine;
using VContainer;

namespace Code.UI.Actions;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class CherriesText : MonoBehaviour
{
    [Inject] private IProgress _progress;

    private TextMeshProUGUI _text;

    private void Awake() =>
        _text = GetComponent<TextMeshProUGUI>();

    private void Start()
    {
        _progress.Persistant.Cherries.Changed += OnCherriesChanged;
        OnCherriesChanged();
    }

    private void OnDestroy() =>
        _progress.Persistant.Cherries.Changed -= OnCherriesChanged;

    private void OnCherriesChanged()
    {
        int cherries = _progress.Persistant.Cherries;
        _text.text = cherries.ToString();
    }
}
