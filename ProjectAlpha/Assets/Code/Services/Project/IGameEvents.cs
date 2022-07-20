using Code.Triggers;

namespace Code.Services;

public interface IGameEvents
{
    GameStartedTrigger GameStart { get; }
}
