using System.Threading;
using Code.Services;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Stick;
using Code.Services.Infrastructure;
using Code.Services.UI.Game;
using Code.Settings;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class EndGameState : IState<EndGameState.Arguments>
{
    public readonly record struct Arguments(IHero Hero, IStick Stick);

    private readonly ICamera _camera1;
    private readonly GameUIController _gameUIController;
    private readonly GameSettings _settings;
    private readonly CancellationToken _token;

    public EndGameState(ICamera camera1, GameUIController gameUIController, GameSettings settings, CancellationToken token)
    {
        _camera1 = camera1;
        _gameUIController = gameUIController;
        _settings = settings;
        _token = token;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        RotateStickDown(args.Stick);
        await Delay(_settings.DelayBeforeFalling);
        await FallHero(args.Hero);
        await Delay(_settings.DelayBeforeEndGame);
        await PunchCamera();
        ShowGameOver();
    }

    private static void RotateStickDown(IStick stick) =>
        stick.RotateDownAsync().Forget();

    private UniTask Delay(int delay) =>
        UniTask.Delay(delay, cancellationToken: _token);

    private UniTask FallHero(IHero hero) =>
        hero.FallAsync(_camera1.Borders.Bot - hero.Borders.Height);

    private UniTask PunchCamera() =>
        _camera1.Punch(_token);

    private void ShowGameOver() =>
        _gameUIController.ShowGameOver();
}
