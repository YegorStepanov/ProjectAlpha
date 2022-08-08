using System.Threading;
using Code.Common;
using Cysharp.Threading.Tasks;

namespace Code.Services.Entities;

public sealed class StickNull : IStick
{
    public static StickNull Default => new();

    private StickNull() { }

    public Borders Borders => Borders.Infinity;

    public bool IsStickArrowOn(IEntity entity) => true;
    public void Increasing(CancellationToken stopToken) { }
    public UniTask RotateAsync() => UniTask.CompletedTask;
    public UniTask RotateDownAsync() => UniTask.CompletedTask;
}
