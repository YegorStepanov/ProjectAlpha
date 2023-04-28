using System.Threading;
using Code.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities
{
    public interface IHero : IEntity
    {
        bool IsFlipped { get; }
        UniTask MoveAsync(float destinationX, CancellationToken token);
        UniTask FallAsync(float destinationY);
        UniTask KickAsync();
        void Squatting(CancellationToken stopToken);
        void Flip();
        void SetPosition(Vector2 position, Relative relative);
    }
}
