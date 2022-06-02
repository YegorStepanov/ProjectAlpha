using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface ICherry
{
    Borders Borders { get; }
    UniTask MoveRandomlyAsync(IPlatform leftPlatform, float rightPlatformLeftBorder);
    void PickUp();
}