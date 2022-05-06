using Code.Services;
using Code.States;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameStart : IStartable
{
    private readonly GameStateMachine _gameStateMachine;

    public GameStart(GameStateMachine gameStateMachine) =>
        _gameStateMachine = gameStateMachine;

    public void Start()
    {
        _gameStateMachine.Enter<BootstrapState>();
    }
}