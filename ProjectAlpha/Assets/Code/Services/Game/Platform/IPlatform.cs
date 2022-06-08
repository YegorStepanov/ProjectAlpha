using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IPlatform : IEntity
{
    RedPoint RedPoint { get; }
    UniTask MoveAsync(float destinationX);
}