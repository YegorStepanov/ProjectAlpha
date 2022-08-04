using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.UI;

public sealed class LoadingScreen : MonoBehaviour, ILoadingScreen
{
    [SerializeField] private CanvasGroup _loadingScreen;

    [SerializeField, DisableIf("_inEasingTime", 0)]
    private Ease _inEasing = Ease.InOutQuad;
    [SerializeField, Min(0)]
    private float _inEasingTime = 0.1f;

    [SerializeField, DisableIf("_outEasingTime", 0)]
    private Ease _outEasing = Ease.InOutQuad;
    [SerializeField, Min(0)]
    private float _outEasingTime = 0.25f;

    private Tween _fadeIn;
    private CancellationToken _token;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public async UniTask FadeInAsync()
    {
        gameObject.SetActive(true);
        _loadingScreen.blocksRaycasts = true;

        _fadeIn = _loadingScreen
            .DOFade(1f, _inEasingTime)
            .From(0)
            .SetEase(_inEasing);

        await _fadeIn.WithCancellation(_token);
    }

    public async UniTask FadeOutAsync()
    {
        if (_fadeIn != null)
            await _fadeIn.WithCancellation(_token);

        _fadeIn = null;

        _loadingScreen.blocksRaycasts = false;

        await _loadingScreen
            .DOFade(0f, _outEasingTime)
            .SetEase(_outEasing)
            .WithCancellation(_token);

        gameObject.SetActive(false);
    }
}
