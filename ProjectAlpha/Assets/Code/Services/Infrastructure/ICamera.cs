using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Infrastructure;

public interface ICamera : IEntity
{
    public void RestoreInitialPosX();
    UniTask ChangeBackgroundAsync();
    UniTask MoveBackgroundAsync();
    UniTask PunchAsync();
    UniTask MoveAsync(Vector2 destination);
}
