using Code.Services;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.States;

public sealed class StickControlState : IState<StickControlState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IPlatform NextPlatform, IHero Hero, ICherry Cherry);

    private readonly StickSpawner _stickSpawner;
    private readonly IInputManager _inputManager;
    private readonly GameUIController _gameUIController;

    public StickControlState(StickSpawner stickSpawner, IInputManager inputManager, GameUIController gameUIController)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
        _gameUIController = gameUIController;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        (IPlatform currentPlatform, IPlatform nextPlatform, IHero hero, ICherry cherry) = args;

        IStick stick = await CreateStick(currentPlatform);
        await IncreaseStick(stick, hero);
        await hero.KickAsync();
        await stick.RotateAsync();
        HandleRedPointHit(stick, nextPlatform);

        if (stick.IsStickArrowOn(nextPlatform))
        {
            stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
                new(currentPlatform, nextPlatform, hero, stick, cherry));
        }
        else
        {
            stateMachine.Enter<MoveHeroToGameOverState, MoveHeroToGameOverState.Arguments>(
                new(currentPlatform, nextPlatform, hero, stick, cherry));
        }
    }

    private UniTask<IStick> CreateStick(IPlatform platform)
    {
        return _stickSpawner.CreateAsync(platform.Borders.RightTop);
    }

    private async UniTask IncreaseStick(IStick stick, IHero hero)
    {
        await _inputManager.WaitClick();
        using CancellationTokenDisposable cts = new();

        hero.Squatting(cts.Token);
        stick.Increasing(cts.Token);

        await _inputManager.WaitClickRelease();
    }

    private void HandleRedPointHit(IStick stick, IPlatform platform)
    {
        if (stick.IsStickArrowOn(platform.PlatformRedPoint))
            _gameUIController.HitRedPoint(platform.PlatformRedPoint.Borders.Center);
    }
}
