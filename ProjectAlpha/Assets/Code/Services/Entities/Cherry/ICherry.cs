using Cysharp.Threading.Tasks;

namespace Code.Services.Entities;

public interface ICherry : IEntity
{
    UniTask MoveAsync(float destinationX);
    void PickUp();
}
