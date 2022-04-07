using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Code.Game.States;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Code.Project
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

        public async UniTask HideAsync() =>
            await loadingScreen.DOFade(0, duration).WithCancellation(token);
    }
}