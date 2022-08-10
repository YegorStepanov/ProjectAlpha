using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface ICamera : IEntity
{
    public CameraBackground Background {get;}
    void SetPosition(Vector2 position);
    UniTask PunchAsync();
    UniTask MoveAsync(Vector2 destination);
}
