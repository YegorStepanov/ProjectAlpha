using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IPlatform : IEntity
{
    IRedPoint RedPoint { get; }
    UniTask MoveAsync(float destinationX);
}