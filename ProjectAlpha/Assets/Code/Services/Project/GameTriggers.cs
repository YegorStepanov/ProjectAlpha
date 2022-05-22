using Code.Triggers;

namespace Code.Services;

public sealed class GameTriggers
{
    public GameStartTrigger OnGameStarted { get; } = new();
}