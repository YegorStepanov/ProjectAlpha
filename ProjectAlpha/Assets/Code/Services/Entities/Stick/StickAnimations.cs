using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services.Entities;

public class StickAnimations : IStickAnimations
{
    public async UniTaskVoid Increasing(Transform transform, float speed, CancellationToken token) => await transform
        .DOScaleY(float.MaxValue, speed)
        .SetSpeedBased()
        .WithCancellation(token);

    public UniTask Rotate(Transform transform, Vector3 endRotation, float speed, float delay, CancellationToken token) => transform
        .DORotate(endRotation, speed)
        .SetEase(Ease.InQuad)
        .SetDelay(delay)
        .WithCancellation(token);
}
