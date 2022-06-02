using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface ICherry : IEntity
{
    UniTask MoveRandomlyAsync(IPlatform leftPlatform, float rightPlatformLeftBorder);
    void PickUp();
}