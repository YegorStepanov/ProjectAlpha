using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities.Hero;

public interface IHeroAnimations
{
    UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token);
    UniTask Fall(Transform transform, float destinationY, float speed, CancellationToken token);
    UniTaskVoid Squatting(Transform transform, float squatOffset, float squatSpeed, CancellationToken token);
}
