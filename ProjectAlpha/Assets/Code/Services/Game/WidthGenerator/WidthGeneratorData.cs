using UnityEngine;

namespace Code.Services;

public abstract class WidthGeneratorData : ScriptableObject
{
    public abstract IWidthGenerator Create();
}