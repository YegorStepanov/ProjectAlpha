using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Data.PositionGenerator;
using Code.Data.WidthGenerator;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Entities.Stick;
using Code.Services.UI.Game;

namespace Code.Addresses;

public static class GameAddress
{
    public static readonly Address<Hero> Hero = new("Hero");
    public static readonly Address<Stick> Stick = new("Stick");
    public static readonly Address<Platform> Platform = new("Platform");
    public static readonly Address<Cherry> Cherry = new("Cherry");

    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<PlatformPositionGenerator> PlatformPositionGenerator = new("Platform Position Generator");
    public static readonly Address<CherryPositionGenerator> CherryPositionGenerator = new("Cherry Position Generator");

    public static readonly Address<GameUIView> GameUI = new("Game UI");
    public static readonly Address<RedPointHitGameAnimation> Plus1Notification = new("Plus 1 Notification");
}
