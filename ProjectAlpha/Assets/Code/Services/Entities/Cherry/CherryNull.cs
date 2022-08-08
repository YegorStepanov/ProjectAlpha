using Code.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities;

public sealed class CherryNull : ICherry
{
    public static CherryNull Default => new();

    private CherryNull() { }

    public Borders Borders => Borders.Infinity;

    public void SetPosition(Vector2 position) { }
    public UniTask MoveAsync(float destinationX) => UniTask.CompletedTask;
    public void PickUp() { }
}
