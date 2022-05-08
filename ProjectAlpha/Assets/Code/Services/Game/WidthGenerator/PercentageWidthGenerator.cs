using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "SO/Percentage Width Generator")]
public sealed class PercentageWidthGenerator : WidthGenerator //todo: split to 2 classes
{
    [SerializeField, Min(0f)]
    private float _minWidth = 0.25f;

    [SerializeField]
    private float _maxWidth = 2f;

    [SerializeField, Range(0, 1)]
    private float _reductionRatioPerStep = 0.1f;

    [SerializeField, Min(0f)]
    private float _minThreshold = 0.25f;

    [SerializeField, Min(0f)]
    private float _maxThreshold = 2f;

    private Ratio _currentRatio;

    //Don't forget: when Domain Reload is disabled, SO events are not called and these instances are not reset 
    private void Awake() =>
        Reset();

    public override void Reset() =>
        _currentRatio = new Ratio(1f, 1f);

    public override float NextWidth()
    {
        float width = NextWidth(_currentRatio);
        width = LimitByThreshold(width);
        _currentRatio = NextRatio(_currentRatio);
        return width;
    }

    private float NextWidth(Ratio ratio) =>
        Random.Range(_minWidth * ratio.Min, _maxWidth * ratio.Max);

    private Ratio NextRatio(Ratio ratio) =>
        ratio * (1f - _reductionRatioPerStep);

    private float LimitByThreshold(float width)
    {
        if (width < _minThreshold) return _minThreshold;
        if (width > _maxThreshold) return _maxThreshold;
        return width;
    }
}