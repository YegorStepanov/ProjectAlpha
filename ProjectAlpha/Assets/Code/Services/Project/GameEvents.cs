using System;
using Code.Infrastructure;
using Code.Triggers;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Code.Services;

public sealed class GameEvents
{
    public GameStartedTrigger GameStarted { get; private set; }

    public GameEvents()
    {
        Init();
    }

    private void Init()
    {
        var enumerable = PlatformInfo.IsEditor
            ? UniTaskAsyncEnumerable.EveryUpdate()
            : ThrowExceptionEnumerable();

        GameStarted = new GameStartedTrigger(enumerable);
    }

    private static IUniTaskAsyncEnumerable<AsyncUnit> ThrowExceptionEnumerable()
    {
        InvalidOperationException exception =
            new("GameTriggers.GameStarted is not bounded to the correct stream, maybe you loaded from the wrong scene");

        return UniTaskAsyncEnumerable.Throw<InvalidOperationException>(exception).Cast<AsyncUnit>();
    }
}
