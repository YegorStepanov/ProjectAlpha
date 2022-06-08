using Code.Triggers;
using Cysharp.Threading.Tasks.Linq;

namespace Code.Services;

public sealed class GameEvents
{
    public GameStartedTrigger GameStarted { get; }

    public GameEvents()
    {
#if UNITY_EDITOR
        var events = UniTaskAsyncEnumerable.EveryUpdate();
#else
        var events = UniTaskAsyncEnumerable.Throw<System.InvalidOperationException>(
                new System.InvalidOperationException(
                    "GameTriggers.GameStarted is not bounded to the correct stream, maybe you loaded from the wrong scene"))
            .Cast<Cysharp.Threading.Tasks.AsyncUnit>();

#endif
        GameStarted = new GameStartedTrigger(events);
    }
}