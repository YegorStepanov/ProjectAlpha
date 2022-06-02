using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatformController CurrentPlatform,
        IPlatformController NextPlatform,
        IStickController Stick,
        IHeroController Hero,
        ICherryController Cherry);

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (IsStickOnPlatform(args.Stick, args.NextPlatform))
        {
            stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
                new(false, args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry, args.Stick));
        }
        else
        {
            stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
                new(true, args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry, args.Stick));
        }

        await UniTask.CompletedTask;
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