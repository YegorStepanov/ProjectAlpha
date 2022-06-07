using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class HeroMovementToGameOverState : BaseHeroMovementState, IState<HeroMovementToGameOverState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        IStick Stick,
        ICherry Cherry);

    public HeroMovementToGameOverState(InputManager inputManager, GameMediator gameMediator) :
        base(inputManager, gameMediator) { }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();

        _ = HeroFlipsOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token);
        UniTask collect = HeroCollectsCherry(args.Hero, args.Cherry, cts.Token);

        float destinationX = args.Stick.Borders.Right;

        await UniTask.WhenAny(
            HeroCollides(args.Hero, args.CurrentPlatform, cts.Token),
            MoveHero(destinationX, args.Hero, cts.Token));

        cts.Cancel();

        TryIncreaseCherryCount(collect);

        await GameOver(args.Hero);
    }
}