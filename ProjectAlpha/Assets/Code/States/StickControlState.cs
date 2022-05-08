using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class StickControlState : IArgState<StickControlState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform,
        IHeroController Hero);

    private readonly InputManager _inputManager;

    private readonly StickSpawner _stickSpawner;

    public StickControlState(StickSpawner stickSpawner, InputManager inputManager)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        Vector2 stickPosition = new(args.CurrentPlatform.Borders.Right, args.CurrentPlatform.Borders.Top);
        IStickController stick = _stickSpawner.CreateStick(stickPosition);

        await _inputManager.NextMouseClick();
        stick.StartIncreasing();

        await _inputManager.NextMouseRelease();
        stick.StopIncreasing();

        await stick.RotateAsync();

        stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new MoveHeroToNextPlatformState.Arguments(args.CurrentPlatform, args.NextPlatform, stick, args.Hero));
    }

    public void Exit() { }
}