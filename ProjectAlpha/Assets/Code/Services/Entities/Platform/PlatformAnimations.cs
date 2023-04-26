using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services.Entities
{
    public class PlatformAnimations : IPlatformAnimations
    {
        public UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token) => transform
            .DOMoveX(destinationX, speed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(token);

        public UniTask FadeOut(SpriteRenderer sprite, float speed, CancellationToken token) => sprite
            .DOFade(0f, speed)
            .WithCancellation(token);
    }
}
