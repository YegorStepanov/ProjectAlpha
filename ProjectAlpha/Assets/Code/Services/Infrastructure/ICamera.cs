using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services.Infrastructure;

public interface ICamera : IEntity
{
    RawImage BackgroundImage { get; }
    UniTask PunchAsync();
    UniTask MoveAsync(Vector2 destination);
}
