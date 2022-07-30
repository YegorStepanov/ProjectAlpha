using Cysharp.Threading.Tasks;

namespace Code.Services.Entities.Platform;

public interface IPlatform : IEntity
{
    IPlatformRedPoint PlatformRedPoint { get; }
    UniTask MoveAsync(float destinationX);
}
