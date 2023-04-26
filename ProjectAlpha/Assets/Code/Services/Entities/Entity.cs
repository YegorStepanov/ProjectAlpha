using System.Threading;
using Code.Common;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity, IPositionShifter
    {
        protected CancellationToken DestroyToken;

        public abstract Borders Borders { get; }

        protected virtual void Awake() =>
            DestroyToken = this.GetCancellationTokenOnDestroy();

        public void ShiftPosition(Vector2 distance) =>
            transform.position = transform.position.ShiftXY(distance);

        public void SetPosition(Vector2 position, Relative relative) =>
            transform.position = position.Shift(Borders, relative).WithZ(transform.position.z);
    }
}
