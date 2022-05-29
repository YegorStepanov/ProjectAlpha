using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Animations.Game;

public sealed class ShowStartHelpAnimation : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    
    private void Awake() =>
        _canvas.enabled = false;

    public void Show()
    {
        _canvas.enabled = true;
        _text.DOFade(1f, 1f);
    }

    public async UniTask HideAsync(CancellationToken token)
    {
        await _text.DOFade(0f, 1f).WithCancellation(token);

        _canvas.enabled = false;
    }
}