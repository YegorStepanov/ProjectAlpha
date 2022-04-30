using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IStickController
{
    Borders Borders { get; }

    public void StartIncreasing();
    public void StopIncreasing();

    UniTask RotateAsync();
}