using Code.AddressableAssets;
using Code.Services;

namespace Code;

public static class GameAddress
{
    public static readonly Address<HeroController> Hero = new("Hero");
    public static readonly Address<StickController> Stick = new("Stick");
    public static readonly Address<PlatformController> Platform = new("Platform");

    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<PositionGeneratorData> PositionGenerator = new("Position Generator");
}