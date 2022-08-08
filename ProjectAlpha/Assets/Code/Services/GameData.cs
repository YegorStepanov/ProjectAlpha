using Code.Services.Entities;

namespace Code.Services;

public readonly record struct GameData(
    IHero Hero,
    IPlatform CurrentPlatform,
    IPlatform NextPlatform,
    ICherry Cherry,
    IStick Stick,
    GameHeight GameHeight);
