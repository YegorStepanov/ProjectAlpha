using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IStick : IEntity
{
    Vector2 ArrowPosition { get; }
    public void StartIncreasing();
    public void StopIncreasing();
    UniTask RotateAsync();
}