using UnityEngine;

namespace Code.Data.WidthGenerator;

public abstract class WidthGeneratorData : ScriptableObject
{
    public abstract IWidthGenerator Create();
}
