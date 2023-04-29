using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities
{
    public interface ICherryAnimations
    {
        UniTask Move(Transform transform, float destinationX, float speed, CancellationToken token);
    }
}