using UnityEngine;

namespace Code.Services.Entities;

public interface IPositionShifter
{
    public void ShiftPosition(Vector2 distance);
}
