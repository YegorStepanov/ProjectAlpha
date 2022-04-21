using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Code.Common;
using Code.Extensions;
using Code.Game;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public interface IStickController
    {
        Borders Borders { get; }

        public void StartIncreasing();
        public void StopIncreasing();

        UniTask RotateAsync();
    }

    public sealed class StickController : MonoBehaviour, IStickController
    {
        [SerializeField] private Transform stick;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private TweenerCore<Vector3, Vector3, VectorOptions> increasingTweener;

        public Borders Borders => spriteRenderer.bounds.AsBorders();

        public float Width
        {
            get => stick.localScale.x * Borders.Width;
            set => stick.localScale = stick.localScale.WithX(value) / Borders.Width;
        }

        public Vector2 Position
        {
            get => stick.position;
            set => stick.position = value;
        }

        private void Awake() =>
            stick.localScale = stick.localScale.WithY(0f);

        public void StartIncreasing() =>
            increasingTweener = stick.DOScaleY(float.MaxValue, 20f)
                .SetSpeedBased();

        public void StopIncreasing() =>
            increasingTweener?.Kill();

        public async UniTask RotateAsync() =>
            await stick.DORotate(new Vector3(0, 0, -90), 0.4f)
                .SetEase(Ease.InQuart)
                .WithCancellation(this.GetCancellationTokenOnDestroy()); // .SetSpeedBased()

        public class Pool : MonoMemoryPool<StickController> { }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector2(Borders.Left, Borders.Top), 0.02f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector2(Borders.Right, Borders.Top), 0.02f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector2(Borders.Left, Borders.Bottom), 0.02f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(new Vector2(Borders.Right, Borders.Bottom), 0.02f);
        }
    }
}