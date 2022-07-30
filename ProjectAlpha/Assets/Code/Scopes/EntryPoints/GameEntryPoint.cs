using System;
using Code.Services;
using Code.Services.Data;
using Code.Services.UI.Game;
using Code.States;
using VContainer.Unity;

namespace Code.Scopes.EntryPoints;

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
