using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class StickNull : IStick
{
    public Borders Borders => Borders.Infinity;

    public bool IsStickArrowOn(IEntity entity) => true;
    public void Increasing(CancellationToken token) { }
    public UniTask RotateAsync() => UniTask.CompletedTask;
    public UniTask RotateDownAsync() => UniTask.CompletedTask;
}
