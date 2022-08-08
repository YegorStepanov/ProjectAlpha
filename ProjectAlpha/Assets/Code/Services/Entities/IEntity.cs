using Code.Common;
using UnityEngine;

namespace Code.Services.Entities;

public interface IEntity
{
    Borders Borders { get; }
    public void SetPosition(Vector2 position);
}
