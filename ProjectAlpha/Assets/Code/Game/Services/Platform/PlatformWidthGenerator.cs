using Sirenix.OdinInspector;

namespace Code.Game
{
    public abstract class PlatformWidthGenerator : SerializedScriptableObject
    {
        public abstract void Reset();
        public abstract float NextWidth();
    }
}