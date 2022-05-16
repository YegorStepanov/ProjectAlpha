using Code.AddressableAssets;
using Code.Services;

namespace Code;

public static class GameAddress
{
    public static readonly Address<HeroController> Hero = new("Hero");
    public static readonly Address<StickController> Stick = new("Stick");
    public static readonly Address<PlatformController> Platform = new("Platform");
}