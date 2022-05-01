using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services;

public sealed class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private float _duration = 0.3f;

    private CancellationToken _token;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void Show()
    {
        gameObject.SetActive(true);
        _loadingScreen.alpha = 1f;
    }

    public UniTask HideAsync() =>
        _loadingScreen.DOFade(0f, _duration).WithCancellation(_token);
}