using Code.Services;
using Code.States;
using Zenject;

namespace Code.Scopes;

public sealed class GameInitializer : IInitializable
{
    private readonly GameStateMachine gameStateMachine;

    public GameInitializer(GameStateMachine gameStateMachine) =>
        this.gameStateMachine = gameStateMachine;

    public void Initialize()
    {
        gameStateMachine.Enter<BootstrapState>();
    }
}