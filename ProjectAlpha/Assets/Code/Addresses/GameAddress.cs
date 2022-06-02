using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Services;
using Code.Services.Game.UI;

namespace Code;

public static class GameAddress
{
    public static readonly Address<Hero> Hero = new("Hero");
    public static readonly Address<Stick> Stick = new("Stick");
    public static readonly Address<Platform> Platform = new("Platform");
    public static readonly Address<Cherry> Cherry = new("Cherry");

    public static readonly Address<WidthGeneratorData> WidthGenerator = new("Width Generator");
    public static readonly Address<NextPositionGenerator> NextPositionGenerator = new("Next Position Generator");

    public static readonly Address<GameUI> GameUI = new("Game UI");
    public static readonly Address<RedPointHitGameAnimation> Plus1Notification = new("Plus 1 Notification");
}