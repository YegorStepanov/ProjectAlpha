using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IPlatform : IEntity
{
    Borders RedPointBorders { get; }
    UniTask MoveAsync(float destinationX);
    bool IsInsideRedPoint(float point);
    UniTask FadeOutRedPointAsync();
}