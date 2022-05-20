using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "Data/Width Generator/Const", fileName = "Width Generator (Const)")]
public sealed class ConstWidthGeneratorData : WidthGeneratorData
{
    [Min(0f)]
    public float Width = 2f;

    public override IWidthGenerator Create() => new ConstWidthGenerator(this);
}

public sealed class ConstWidthGenerator : IWidthGenerator
{
    private readonly ConstWidthGeneratorData _data;

    public ConstWidthGenerator(ConstWidthGeneratorData data) =>
        _data = data;

    public float NextWidth() =>
        _data.Width;
}