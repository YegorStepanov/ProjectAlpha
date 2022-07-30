using Code.Common;
using UnityEngine;

namespace Code.Data.WidthGenerator;

[CreateAssetMenu(menuName = "Data/Width Generator/Percentage", fileName = "Width Generator (Percentage)")]
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

public sealed class PercentageWidthGenerator : IWidthGenerator
{
    private readonly PercentageWidthGeneratorData _data;

    private Ratio _currentRatio = new(1f, 1f);

    public PercentageWidthGenerator(PercentageWidthGeneratorData data) =>
        _data = data;

    public float NextWidth()
    {
        float width = NextWidth(_currentRatio);
        width = LimitByThreshold(width);
        _currentRatio = NextRatio(_currentRatio);
        return width;
    }

    private float NextWidth(Ratio ratio) =>
        Random.Range(_data.MinWidth * ratio.Min, _data.MaxWidth * ratio.Max);

    private Ratio NextRatio(Ratio ratio) =>
        ratio * (1f - _data.ReductionRatioPerStep);

    private float LimitByThreshold(float width)
    {
        if (width < _data.MinThreshold) return _data.MinThreshold;
        if (width > _data.MaxThreshold) return _data.MaxThreshold;
        return width;
    }
}
