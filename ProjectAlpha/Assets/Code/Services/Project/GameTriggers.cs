using Code.Triggers;

namespace Code.Services;

public sealed class GameTriggers
{
    public StartGameTrigger StartGameClicked { get; } = new();
}