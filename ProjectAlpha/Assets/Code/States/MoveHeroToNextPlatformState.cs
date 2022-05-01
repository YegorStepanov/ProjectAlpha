using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IArgState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatformController CurrentPlatform, 
        IPlatformController NextPlatform,
        IStickController Stick);

    private readonly IHeroController _hero;
    private readonly PlatformSpawner _platformSpawner;
    private readonly GameStateMachine _stateMachine;

    public MoveHeroToNextPlatformState(
        GameStateMachine stateMachine,
        IHeroController hero,
        PlatformSpawner platformSpawner)
    {
        _stateMachine = stateMachine;
        _hero = hero;
        _platformSpawner = platformSpawner;
    }

    public async UniTaskVoid EnterAsync(Arguments args)
    {
        if (IsStickOnPlatform(args.Stick, args.NextPlatform))
        {
            _stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                new GameStartState.Arguments(args.NextPlatform));
        }
        else
        {
            await _hero.MoveAsync(args.Stick.Borders.Right);
            await _hero.FellAsync();

            //earthshake screen
        }
    }

    public void Exit() { }

    private static bool IsStickOnPlatform(IStickController stick, IPlatformController platform)
    {
        var stickPosX = stick.Borders.Right;

        if (stickPosX < platform.Borders.Left)
            return false;

        if (stickPosX > platform.Borders.Right)
            return false;

        return true;
    }
}