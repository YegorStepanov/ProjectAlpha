using UnityEngine;

namespace Code.Services;

public abstract class PositionGeneratorData : ScriptableObject
{
    public abstract IPositionGenerator Create();
}