using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class StickControlState : IArgState<StickControlState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform,
        IHeroController Hero);

    private readonly InputManager _inputManager;

    private readonly GameStateMachine _stateMachine;
    private readonly StickSpawner _stickSpawner;

    public StickControlState(GameStateMachine stateMachine, StickSpawner stickSpawner, InputManager inputManager)
    {
        _stateMachine = stateMachine;
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
    }

    public async UniTaskVoid EnterAsync(Arguments args)
    {
        float positionX = args.CurrentPlatform.Borders.Right;
        IStickController stick = _stickSpawner.Spawn(positionX);

        await _inputManager.NextMouseClick();
        stick.StartIncreasing();

        await _inputManager.NextMouseRelease();
        stick.StopIncreasing();

        await stick.RotateAsync();

        _stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new MoveHeroToNextPlatformState.Arguments(args.CurrentPlatform, args.NextPlatform, stick, args.Hero));
    }

    public void Exit() { }
}