using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Services;
using Code.Services.Game.UI;

namespace Code;

public static class GameAddress
{
    public static readonly Address<HeroController> Hero = new("Hero");
    public static readonly Address<StickController> Stick = new("Stick");
    public static readonly Address<PlatformController> Platform = new("Platform");
    public static readonly Address<CherryController> Cherry = new("Cherry");

    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<PositionGeneratorData> PositionGenerator = new("Position Generator");

    public static readonly Address<GameUIMediator> GameUI = new("Game UI");
    public static readonly Address<RedPointHitGameAnimation> Plus1Notification = new("Plus 1 Notification");
}