using Code.AddressableAssets;
using Code.Services;

namespace Code;

public static class DataAddress
{
    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<PositionGeneratorData> PositionGenerator = new("Position Generator");
}