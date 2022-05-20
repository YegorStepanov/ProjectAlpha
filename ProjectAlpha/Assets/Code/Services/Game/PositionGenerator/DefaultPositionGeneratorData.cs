using UnityEngine;

namespace Code.Services;

[CreateAssetMenu(menuName = "Data/Position Generator/Default", fileName = "Position Generator (Default)")]
public sealed class DefaultPositionGeneratorData : PositionGeneratorData
{
    public float LeftOffset = 0.5f;

    public override IPositionGenerator Create() => new DefaultPositionGenerator(this);
}

public sealed class DefaultPositionGenerator : IPositionGenerator
{
    private readonly DefaultPositionGeneratorData _data;

    public DefaultPositionGenerator(DefaultPositionGeneratorData data) =>
        _data = data;

    public float NextPosition(IPlatformController currentPlatform, IPlatformController nextPlatform)
    {
        float halfWidth = nextPlatform.Borders.Width / 2f;

        float position = Random.Range(
            currentPlatform.Borders.Right + halfWidth + _data.LeftOffset,
            nextPlatform.Borders.Left - halfWidth);

        return position;
    }
}