using UnityEngine;

namespace Code.Game
{
    public abstract class WidthGenerator : ScriptableObject
    {
        public abstract void Reset();
        public abstract float NextWidth();
    }
}