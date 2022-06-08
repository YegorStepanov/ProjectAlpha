using System.Threading;
using System.Threading.Tasks;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class StickControlState : IState<StickControlState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IHero Hero,
        ICherry Cherry);

    private readonly StickSpawner _stickSpawner;
    private readonly InputManager _inputManager;
    private readonly GameMediator _gameMediator;

    public StickControlState(StickSpawner stickSpawner, InputManager inputManager, GameMediator gameMediator)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        IStick stick = await _stickSpawner.CreateAsync(args.CurrentPlatform.Borders.RightTop);

        await _inputManager.NextClick();
        await IncreaseStick(stick);

        await args.Hero.KickAsync();
        await stick.RotateAsync();

        if (stick.IsStickArrowOn(args.NextPlatform.RedPoint))
            _gameMediator.OnRedPointHit(args.NextPlatform.RedPoint.Borders.Center); //rename parameter name

        stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new(args.CurrentPlatform, args.NextPlatform, stick, args.Hero, args.Cherry));
    }

    private async Task IncreaseStick(IStick stick)
    {
        var cts = new CancellationTokenSource();
        stick.StartIncreasingAsync(cts.Token).Forget();

        await _inputManager.NextRelease();
        cts.Cancel();
    }
}