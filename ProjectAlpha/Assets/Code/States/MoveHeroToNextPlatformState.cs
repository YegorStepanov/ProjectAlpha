using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IArgState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform,
        IPlatformController NextPlatform,
        IStickController Stick, IHeroController Hero);

    private readonly PlatformSpawner _platformSpawner;
    private readonly GameStateMachine _stateMachine;

    public MoveHeroToNextPlatformState(GameStateMachine stateMachine, PlatformSpawner platformSpawner)
    {
        _stateMachine = stateMachine;
        _platformSpawner = platformSpawner;
    }

    public async UniTaskVoid EnterAsync(Arguments args)
    {
        if (IsStickOnPlatform(args.Stick, args.NextPlatform))
        {
            _stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                new GameStartState.Arguments(args.NextPlatform, args.Hero));
        }
        else
        {
            await args.Hero.MoveAsync(args.Stick.Borders.Right);
            await args.Hero.FellAsync();

            //earthshake screen
        }
    }

    public void Exit() { }

    private static bool IsStickOnPlatform(IStickController stick, IPlatformController platform)
    {
        float stickPosX = stick.Borders.Right;

        if (stickPosX < platform.Borders.Left)
            return false;

        if (stickPosX > platform.Borders.Right)
            return false;

        return true;
    }
}