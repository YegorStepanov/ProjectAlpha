using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public abstract class Entity : MonoBehaviour, IEntity
{
    protected CancellationToken token;
    
    public abstract Borders Borders { get; }

    protected virtual void Awake() =>
        token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 position, Relative relative = Relative.Center) =>
        transform.position = position.Shift(Borders, relative);

    public void SetWidth(float width) =>
        transform.localScale = transform.localScale.WithX(width / Borders.Width);
}