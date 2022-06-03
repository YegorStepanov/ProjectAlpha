using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Code.States;

public sealed class HeroMovementToPlatformState : BaseHeroMovementState, IState<HeroMovementToPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        IStick Stick, //remove 
        [CanBeNull] ICherry Cherry);

    public HeroMovementToPlatformState(InputManager inputManager, GameMediator gameMediator) :
        base(inputManager, gameMediator) { }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();
        
        _ = HeroFlipsOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token);
        UniTask collect = HeroCollectsCherry(args.Hero, args.Cherry, cts.Token); //todo

        float destinationX = args.CurrentPlatform.Borders.Right;
        destinationX -= args.Stick.Borders.HalfWidth;
        destinationX -= args.Hero.HandOffset;
        
        (bool isCollided, _) = await UniTask.WhenAny(
            HeroCollides(args.Hero, args.CurrentPlatform, cts.Token),
            MoveHero(destinationX, args.Hero, args.Stick, cts.Token));

        cts.Cancel();

        if (collect.Status == UniTaskStatus.Succeeded)
            args.Cherry?.PickUp();

        if (isCollided)
            await GameOver(args.Hero);
        else
            TryIncreaseCherryCount(collect);

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(new(args.CurrentPlatform, args.Hero, args.Stick));
    }

    public void Exit() { }
}