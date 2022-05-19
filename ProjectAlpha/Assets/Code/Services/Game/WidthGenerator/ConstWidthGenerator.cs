namespace Code.Services;

public sealed class ConstWidthGenerator : IWidthGenerator
{
    private readonly ConstWidthGeneratorData _data;

    public ConstWidthGenerator(ConstWidthGeneratorData data) =>
        _data = data;

    public float NextWidth() =>
        _data.Width;
}