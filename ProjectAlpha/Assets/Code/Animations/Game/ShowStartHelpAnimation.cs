using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Animations.Game
{
    public sealed class ShowStartHelpAnimation : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake() =>
            _canvas.enabled = false;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async UniTaskVoid PlayAsync()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _text.color = _text.color with { a = 0f };
            _canvas.enabled = true;
            await _text.DOFade(1f, 1f).WithCancellation(_cts.Token);
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _text.color = _text.color with { a = 1f };
            await _text.DOFade(0f, 1f).WithCancellation(_cts.Token);
            _canvas.enabled = false;
        }

        public void Hide() =>
            _canvas.enabled = false;
    }
}