using System.Threading;
using Code.Common;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities;

public abstract class Entity : MonoBehaviour, IEntity
{
    protected CancellationToken DestroyToken;

    public abstract Borders Borders { get; }

    protected virtual void Awake() =>
        DestroyToken = this.GetCancellationTokenOnDestroy();

    //todo:
    public virtual void SetPosition(Vector2 position) =>
        transform.position = position.WithZ(transform.position.z);

    public void SetPosition(Vector2 position, Relative relative) =>
        transform.position = position.Shift(Borders, relative).WithZ(transform.position.z);

    public void SetWidth(float width) =>
        transform.localScale = transform.localScale.WithX(width / Borders.Width);
}
