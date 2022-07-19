using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services;

public class HeroAnimations : IHeroAnimations
{
    public UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token) => transform
        .DOMoveX(destinationX, speed)
        .SetEase(Ease.Linear)
        .SetSpeedBased()
        .WithCancellation(token);

    public UniTask Fall(Transform transform, float destinationY, float speed, CancellationToken token) => transform
        .DOMoveY(destinationY, speed)
        .SetEase(Ease.Linear)
        .SetSpeedBased()
        .WithCancellation(token);

    public async UniTask Squat(Transform transform, float squatOffset, float squatSpeed, CancellationToken token)
    {
        Vector3 scale = transform.localScale;

        await transform.DOScaleY(-squatOffset, squatSpeed)
            .SetRelative()
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Yoyo)
            .WithCancellation(token);

        transform.localScale = scale;
    }
}
