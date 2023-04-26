using System.Threading;
using Cysharp.Threading.Tasks;

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
    }
}
