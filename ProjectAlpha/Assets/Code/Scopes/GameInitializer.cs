using Code.Services;
using Code.States;
using Zenject;

namespace Code.Scopes;

public sealed class GameInitializer : IInitializable
{
    private readonly GameStateMachine _gameStateMachine;

    public GameInitializer(GameStateMachine gameStateMachine) =>
        _gameStateMachine = gameStateMachine;

    public void Initialize()
    {
        _gameStateMachine.Enter<BootstrapState>();
    }
}