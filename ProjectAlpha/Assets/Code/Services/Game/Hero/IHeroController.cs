using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IHeroController
{
    float HandOffset { get; } //todo: OffsetToItem/Stick?
    UniTask MoveAsync(float destinationX);
    void TeleportTo(Vector2 destination, Relative relative);
    UniTask FellAsync();
}