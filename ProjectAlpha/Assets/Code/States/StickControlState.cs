using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class StickControlState : IArgState<StickControlState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform,
        IHeroController Hero);

    private readonly InputManager _inputManager;
    private readonly GameUIMediator _gameUI;
    private readonly StickSpawner _stickSpawner;

    public StickControlState(StickSpawner stickSpawner, InputManager inputManager, GameUIMediator gameUI)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
        _gameUI = gameUI;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        Vector2 stickPosition = new(args.CurrentPlatform.Borders.Right, args.CurrentPlatform.Borders.Top);
        IStickController stick = await _stickSpawner.CreateStickAsync(stickPosition);

        await _inputManager.NextMouseClick();
        stick.StartIncreasing();

        await _inputManager.NextMouseRelease();
        stick.StopIncreasing();

        await args.Hero.KickAsync();

        await stick.RotateAsync();

        bool isInside = args.NextPlatform.IsInsideRedPoint(stick.ArrowPosition.x);
        Debug.Log("IsInsideRedPoint: " + isInside);

        if (isInside)
        {
            _gameUI.IncreaseScore();
            _gameUI.ShowRedPointHitNotification(args.NextPlatform.RedPointBorders.Center);
        }

        stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new MoveHeroToNextPlatformState.Arguments(args.CurrentPlatform, args.NextPlatform, stick, args.Hero));
    }

    public void Exit() { }
}