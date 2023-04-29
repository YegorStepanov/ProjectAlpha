using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services.Entities
{
    public class CherryAnimations : ICherryAnimations
    {
        public UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token) => transform
            .DOMoveX(destinationX, speed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(token);
    }
}