using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface ICherryController
{
    Borders Borders { get; }
    UniTask MoveRandomlyAsync(IPlatformController leftPlatform, float rightPlatformLeftBorder);
}