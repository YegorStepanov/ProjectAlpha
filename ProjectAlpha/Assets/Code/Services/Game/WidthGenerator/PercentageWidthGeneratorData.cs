using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "SO/Percentage Width Generator")]
public sealed class PercentageWidthGeneratorData : WidthGeneratorData
{
    [Min(0f)]
    public float MinWidth = 0.25f;
    public float MaxWidth = 2f;
    [Range(0, 1)]
    public float ReductionRatioPerStep = 0.1f;
    [Min(0f)]
    public float MinThreshold = 0.25f;
    [Min(0f)]
    public float MaxThreshold = 2f;

    public override IWidthGenerator Create() => new PercentageWidthGenerator(this);
}