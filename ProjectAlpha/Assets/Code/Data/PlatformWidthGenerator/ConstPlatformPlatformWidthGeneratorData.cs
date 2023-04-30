using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "Data/Platform Width Generator/Const", fileName = "Platform Width Generator (Const)")]
    public sealed class ConstPlatformPlatformWidthGeneratorData : PlatformWidthGeneratorData
    {
        [Min(0f)]
        public float Width = 2f;

        public override IPlatformWidthGenerator Create() => new ConstPlatformWidthGenerator(this);
    }

    public sealed class ConstPlatformWidthGenerator : IPlatformWidthGenerator
    {
        private readonly ConstPlatformPlatformWidthGeneratorData _data;

        public ConstPlatformWidthGenerator(ConstPlatformPlatformWidthGeneratorData data) =>
            _data = data;

        public float NextWidth() =>
            _data.Width;

        public void Reset()
        {
            // Empty
        }
    }
}