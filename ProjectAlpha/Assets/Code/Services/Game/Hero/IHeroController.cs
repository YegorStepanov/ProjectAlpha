using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IHeroController
{
    float HandOffset { get; } //todo: OffsetToItem/Stick?
    UniTask MoveAsync(float destinationX);
    UniTask FellAsync();
}