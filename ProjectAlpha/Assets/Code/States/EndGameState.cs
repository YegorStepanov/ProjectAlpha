using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class EndGameState : IState<EndGameState.Arguments>
{
    public readonly record struct Arguments(IHero Hero, IStick Stick);

    private readonly Camera _camera;
    private readonly GameUIController _gameUIController;
    private readonly GameLoopSettings _settings;
    private readonly CancellationToken _token;

    public EndGameState(Camera camera, GameUIController gameUIController, GameLoopSettings settings, CancellationToken token)
    {
        _camera = camera;
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
        hero.FallAsync(_camera.Borders.Bot - hero.Borders.Height);

    private UniTask PunchCamera() =>
        _camera.Punch(_token);

    private void ShowGameOver() =>
        _gameUIController.ShowGameOver();
}
