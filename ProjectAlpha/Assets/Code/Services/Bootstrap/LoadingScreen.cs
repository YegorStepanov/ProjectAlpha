using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private float _duration = 0.3f;

    private CancellationToken _token;

    [Inject, UsedImplicitly]
    public void Construct(CancellationToken token) =>
        _token = token;

    public void Show()
    {
        gameObject.SetActive(true);
        _loadingScreen.alpha = 1f;
    }

    public UniTask FadeOutAsync() =>
        _loadingScreen.DOFade(0f, _duration).WithCancellation(_token);
}