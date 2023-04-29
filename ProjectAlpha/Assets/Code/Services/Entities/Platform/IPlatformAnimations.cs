using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities
{
    public interface IPlatformAnimations
    {
        UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token);
        UniTask FadeOut(SpriteRenderer sprite, float speed, CancellationToken token);
    }
}