using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "Data/Position Generator/Leftmost", fileName = "Position Generator (Leftmost)")]
public sealed class LeftmostPositionGeneratorData : PositionGeneratorData
{
    public float LeftOffset = 0.5f;

    public override IPositionGenerator Create() => new LeftmostPositionGenerator(this);
}

public sealed class LeftmostPositionGenerator : IPositionGenerator
{
    private readonly LeftmostPositionGeneratorData _data;

    public LeftmostPositionGenerator(LeftmostPositionGeneratorData data) =>
        _data = data;

    public float NextPosition(IPlatformController currentPlatform, IPlatformController nextPlatform)
    {
        float halfWidth = nextPlatform.Borders.Width / 2f;

        float leftmost = currentPlatform.Borders.Right + halfWidth + _data.LeftOffset;
        return leftmost;
    }
}