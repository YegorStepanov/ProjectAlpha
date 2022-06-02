using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IState<MoveHeroToNextPlatformState.Arguments>
{
    private readonly StickSpawner _stickSpawner;

    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IStick Stick,
        IHero Hero,
        ICherry Cherry);

    public MoveHeroToNextPlatformState(StickSpawner stickSpawner)
    {
        _stickSpawner = stickSpawner;
    }
    
    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (args.Stick.Intersect(args.NextPlatform))
        {
            //it should be created AFTER move, to be able to reuse only 2, not 3
            IStick stick = await _stickSpawner.CreateStickAsync(args.NextPlatform.Borders.RightTop);

            stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
                new(false, args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry, stick));
        }
        else
        {
            stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
                new(true, args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry, args.Stick));
        }

        await UniTask.CompletedTask;
    }

    public void Exit() { }
}