using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IPlatformController
{
    Vector2 Position { get; }
    Borders Borders { get; }
    Borders RedPointBorders { get; }
    UniTask MoveAsync(float destinationX);
    bool IsInsideRedPoint(float point);
    UniTask FadeOutRedPointAsync();
}