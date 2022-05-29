using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Animations.Game;

public sealed class RedPointHitAnimation : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake() =>
        _canvas.enabled = false;

    public async UniTask ShowAsync(CancellationToken token)
    {
        _canvas.enabled = true;
        _text.color = _text.color with { a = 1f };
        _text.transform.localScale = Vector3.zero;

        await _text.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 1f)
            .SetEase(Ease.OutElastic, 1, 1f)
            .WithCancellation(token);

        await _text.DOFade(0f, 1f).WithCancellation(token);

        _canvas.enabled = false;
    }
}