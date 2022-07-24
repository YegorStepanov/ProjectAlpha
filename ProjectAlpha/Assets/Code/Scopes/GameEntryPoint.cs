using System;
using Code.Services;
using Code.Services.Game.UI;
using Code.States;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly GameUIController _gameUIController;
    private readonly IProgress _progress;

    public GameEntryPoint(GameStateMachine gameStateMachine, GameUIController gameUIController, IProgress progress)
    {
        _gameStateMachine = gameStateMachine;
        _gameUIController = gameUIController;
        _progress = progress;
    }

    public void Initialize()
    {
        _progress.Session.ScoreChanged += _gameUIController.UpdateScore;
        _progress.Persistant.CherriesChanged += _gameUIController.UpdateCherries;
    }

    public void Dispose()
    {
        _progress.Session.ScoreChanged -= _gameUIController.UpdateScore;
        _progress.Persistant.CherriesChanged -= _gameUIController.UpdateCherries;
    }

    public void Start()
    {
        _gameUIController.UpdateScore();
        _gameUIController.UpdateCherries();

        _gameStateMachine.Enter<StartState>();
    }
}
