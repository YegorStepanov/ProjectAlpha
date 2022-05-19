using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "SO/Const Width Generator")]
public sealed class ConstWidthGeneratorData : WidthGeneratorData
{
    [Min(0f)]
    public float Width = 2f;

    public override IWidthGenerator Create() => new ConstWidthGenerator(this);
}