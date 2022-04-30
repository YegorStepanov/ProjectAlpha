using UnityEngine;

namespace Code.Services;

public abstract class WidthGenerator : ScriptableObject
{
    public abstract void Reset();
    public abstract float NextWidth();
}