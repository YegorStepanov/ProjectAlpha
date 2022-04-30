using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup loadingScreen;
        [SerializeField] private float duration = 0.3f;

        private CancellationToken token;

        private void Awake() =>
            token = this.GetCancellationTokenOnDestroy();

        public void Show()
        {
            gameObject.SetActive(true);
            loadingScreen.alpha = 1;
        }

        public async UniTask HideAsync() { }
        // await loadingScreen.DOFade(0, duration).WithCancellation(token);
    }
}