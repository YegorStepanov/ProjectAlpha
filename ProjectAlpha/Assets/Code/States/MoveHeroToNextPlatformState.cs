using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IStick Stick,
        IHero Hero,
        ICherry Cherry);

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (args.NextPlatform.Contains(args.Stick.ArrowPosition))
        {
            stateMachine.Enter<HeroMovementToPlatformState, HeroMovementToPlatformState.Arguments>(
                new(args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry));
        }
        else
        {
            stateMachine.Enter<HeroMovementToGameOverState, HeroMovementToGameOverState.Arguments>(
                new(args.CurrentPlatform, args.NextPlatform, args.Hero, args.Stick, args.Cherry));
        }

        await UniTask.CompletedTask;
    }

    public void Exit() { }
}