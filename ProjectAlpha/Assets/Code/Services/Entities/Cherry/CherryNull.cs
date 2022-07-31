using Code.Common;
using Cysharp.Threading.Tasks;

namespace Code.Services.Entities;

public sealed class CherryNull : ICherry
{
    public static CherryNull Default => new();

    public Borders Borders => Borders.Infinity;
    public UniTask MoveAsync(float destinationX) => UniTask.CompletedTask;
    public void PickUp() { }
}
