using System.Threading;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Infrastructure;

public interface ICamera : IEntity
{
    public void RestoreInitialPosition();
    UniTask ChangeBackgroundAsync();
    UniTask MoveBackgroundAsync(CancellationToken cancellationToken);
    UniTask Punch(CancellationToken token);
    UniTask MoveAsync(Vector2 destination);
    Vector2 ViewportToWorldPosition(Vector2 viewportPosition);
    float ViewportToWorldPositionX(float viewportPosX);
    float ViewportToWorldPositionY(float viewportPosY);
}
