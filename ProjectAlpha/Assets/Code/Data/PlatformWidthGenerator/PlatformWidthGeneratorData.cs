using UnityEngine;

namespace Code.Data
{
    public abstract class PlatformWidthGeneratorData : ScriptableObject
    {
        public abstract IPlatformWidthGenerator Create();
    }
}
