﻿using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Animations;

public sealed class FloatContinuallyGameAnimation : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private float _hoveringRange;

    [SerializeField] private float _duration;

    private float HalfRange => _hoveringRange / 2;

    [UsedImplicitly]
    private async UniTaskVoid Awake()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();

        await MoveToAnimationStartAsync(token);
        StartCyclicAnimationAsync(token);
    }

    private UniTask MoveToAnimationStartAsync(CancellationToken token) =>
        _transform.DOMoveY(-HalfRange, _duration / 2)
            .SetRelative()
            .WithCancellation(token);

    private void StartCyclicAnimationAsync(CancellationToken token) =>
        _transform.DOMoveY(2 * HalfRange, _duration)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad).WithCancellation(token);
}