using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IPlatformController
{
    Vector2 Position { get; }

    Borders Borders { get; }
    void SetPosition(Vector2 position);
    void SetSize(Vector2 scale);
    UniTask MoveAsync(float destinationX);
    Vector2 GetRelativePosition(Vector2 position, Relative relative);
}
