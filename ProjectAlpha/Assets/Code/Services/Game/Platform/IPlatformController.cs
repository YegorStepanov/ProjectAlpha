using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IPlatformController
{
    Vector2 Position { get; }
    Borders Borders { get; }
    UniTask MoveAsync(float destinationX);
    Vector2 GetRelativePosition(Vector2 position, Relative relative);
    bool IsInsideRedPoint(float point);
    UniTask FadeOutRedPointAsync();
}