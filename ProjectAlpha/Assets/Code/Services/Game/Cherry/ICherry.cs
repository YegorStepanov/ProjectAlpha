using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface ICherry : IEntity
{
    UniTask MoveAsync(float destinationX);
    void PickUp();
}