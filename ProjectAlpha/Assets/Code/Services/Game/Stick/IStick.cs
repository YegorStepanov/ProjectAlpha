using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public interface IStick
{
    Borders Borders { get; }
    Vector2 ArrowPosition { get; }
    public void StartIncreasing();
    public void StopIncreasing();
    UniTask RotateAsync();
}