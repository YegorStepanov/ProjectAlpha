using System;
using Code.Services;
using Code.Services.Data;
using Code.Services.UI;
using Code.States;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly IGameStateMachine _gameStateMachine;
    private readonly GameUIController _gameUIController;
    private readonly IProgress _progress;

    public GameEntryPoint(IGameStateMachine gameStateMachine, GameUIController gameUIController, IProgress progress)
    {
        _gameStateMachine = gameStateMachine;
        _gameUIController = gameUIController;
        _progress = progress;
    }

    void IInitializable.Initialize()
    {
        _progress.Session.ScoreChanged += _gameUIController.UpdateScore;
        _progress.Persistant.CherriesChanged += _gameUIController.UpdateCherries;
    }

    void IDisposable.Dispose()
    {
        _progress.Session.ScoreChanged -= _gameUIController.UpdateScore;
        _progress.Persistant.CherriesChanged -= _gameUIController.UpdateCherries;
    }

    void IStartable.Start()
    {
        _gameUIController.UpdateScore();
        _gameUIController.UpdateCherries();

        _gameStateMachine.Enter<StartState>();
    }
}
