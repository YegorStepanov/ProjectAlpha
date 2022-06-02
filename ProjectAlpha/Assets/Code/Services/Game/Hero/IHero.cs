using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IHero : IEntity
{
    float HandOffset { get; } //todo: OffsetToItem/Stick?
    bool IsFlipped { get; }
    UniTask MoveAsync(float destinationX, CancellationToken token = default);
    UniTask FellAsync();
    UniTask KickAsync();
    void Flip();
}