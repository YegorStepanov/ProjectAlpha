using Code.Services;
using Code.States;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameEntryPoint : IStartable
{
    private readonly GameStateMachine _gameStateMachine;

    public GameEntryPoint(GameStateMachine gameStateMachine) =>
        _gameStateMachine = gameStateMachine;

    public void Start() =>
        _gameStateMachine.Enter<StartState>();
}
