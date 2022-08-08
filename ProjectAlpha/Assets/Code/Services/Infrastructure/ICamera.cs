using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Infrastructure;

public interface ICamera : IEntity
{
    void SetPosition(Vector2 position);
    UniTask ChangeBackgroundAsync();
    UniTask MoveBackgroundAsync();
    UniTask PunchAsync();
    UniTask MoveAsync(Vector2 destination);
}
