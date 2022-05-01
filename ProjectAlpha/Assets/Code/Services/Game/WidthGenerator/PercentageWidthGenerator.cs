using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Services;

[CreateAssetMenu(menuName = "SO/Percentage Width Generator")]
public sealed class PercentageWidthGenerator : WidthGenerator //todo: split to 2 classes
{
    // [SerializeField, MinMaxSlider(0, 1), LabelText("Range of the platform")]
    // private Vector2 platformViewportRange = new(0.1f, 0.5f);
    //
    // [SerializeField, ProgressBar(0, 100), LabelText("Reduction per step (%)")]
    // private float PercentageReductionPerStep = 10f;
    //
    // [SerializeField, MinMaxSlider(0, 1), LabelText("Min/Max threshold")]
    // private Vector2 minMaxViewportThreshold = new(0.1f, 0.2f);

    [SerializeField]
    private float _minWidth = 0.25f;

    [SerializeField]
    private float _maxWidth = 2f;

    [SerializeField, Range(0, 1)]
    private float _reductionRatioPerStep = 0.1f;

    [SerializeField]
    private float _minThreshold = 0.25f;

    [SerializeField]
    private float _maxThreshold = 2f;

    private Ratio _currentRatio;

    public override void Reset() =>
        _currentRatio = new Ratio(1f, 1f);

    public override float NextWidth()
    {
        float width = NextWidth(_currentRatio);
        _currentRatio = NextRatio(_currentRatio);
        _currentRatio = LimitByThreshold(_currentRatio);
        return width;
    }

    private float NextWidth(Ratio ratio) =>
        Random.Range(_minWidth * ratio.Min, _maxWidth * ratio.Max);

    private Ratio NextRatio(Ratio ratio) =>
        ratio * (1f - _reductionRatioPerStep);

    private Ratio LimitByThreshold(Ratio ratio) => new Ratio(
        ratio.Min < _minThreshold ? _minThreshold : ratio.Min,
        ratio.Max < _maxThreshold ? _maxThreshold : ratio.Max);
}