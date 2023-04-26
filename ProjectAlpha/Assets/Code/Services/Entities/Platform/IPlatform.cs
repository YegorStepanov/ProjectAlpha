using Cysharp.Threading.Tasks;

namespace Code.Services.Entities
{
    public interface IPlatform : IEntity
    {
        IPlatformRedPoint PlatformRedPoint { get; }
        UniTask MoveAsync(float destinationX);
    }
}
