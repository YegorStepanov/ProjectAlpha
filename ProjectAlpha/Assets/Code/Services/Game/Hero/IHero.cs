using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IHero : IEntity
{
    float HandOffset { get; } //todo: OffsetToItem/Stick?
    bool IsFlipped { get; }
    UniTask MoveAsync(float destinationX, CancellationToken token);
    UniTask MoveAsync(float destinationX);
    UniTask FallAsync();
    UniTask KickAsync();
    void Flip();
}