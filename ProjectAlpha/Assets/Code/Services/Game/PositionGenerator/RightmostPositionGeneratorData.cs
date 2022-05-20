using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "Data/Position Generator/Rightmost", fileName = "Position Generator (Rightmost)")]
public sealed class RightmostPositionGeneratorData : PositionGeneratorData
{
    public float RightOffset = 0.5f;

    public override IPositionGenerator Create() => new RightmostPositionGenerator(this);
}

public sealed class RightmostPositionGenerator : IPositionGenerator
{
    private readonly RightmostPositionGeneratorData _data;

    public RightmostPositionGenerator(RightmostPositionGeneratorData data) =>
        _data = data;

    public float NextPosition(IPlatformController currentPlatform, IPlatformController nextPlatform)
    {
        float halfWidth = nextPlatform.Borders.Width / 2f;

        float rightmost = nextPlatform.Borders.Left - halfWidth - _data.RightOffset;
        return rightmost;
    }
}