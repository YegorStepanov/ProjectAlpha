using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services.Infrastructure;

public interface ICamera : IEntity
{
    void SetPosition(Vector2 position);
    RawImage BackgroundImage { get; }
    UniTask PunchAsync();
    UniTask MoveAsync(Vector2 destination);
}
