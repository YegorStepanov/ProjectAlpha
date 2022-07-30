using Cysharp.Threading.Tasks;

namespace Code.Services.Entities.Cherry;

public interface ICherry : IEntity
{
    UniTask MoveAsync(float destinationX);
    void PickUp();
}
