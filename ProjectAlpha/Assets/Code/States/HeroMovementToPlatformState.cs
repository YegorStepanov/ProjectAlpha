using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class HeroMovementToPlatformState : BaseHeroMovementState, IState<HeroMovementToPlatformState.Arguments>
{
    private readonly StickSpawner _stickSpawner;

    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        ICherry Cherry);

    public HeroMovementToPlatformState(
        InputManager inputManager, GameMediator gameMediator, StickSpawner stickSpawner) :
        base(inputManager, gameMediator)
    {
        _stickSpawner = stickSpawner;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();

        HeroFlipsOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token).Forget();

        UniTask collect = HeroCollectsCherry(args.Hero, args.Cherry, cts.Token); //todo

        float destinationX = args.CurrentPlatform.Borders.Right;
        destinationX -= _stickSpawner.StickWidth / 2f;
        destinationX -= args.Hero.HandOffset;

        (bool isCollided, _) = await UniTask.WhenAny(
            HeroCollides(args.Hero, args.CurrentPlatform, cts.Token),
            MoveHero(destinationX, args.Hero, cts.Token));

        cts.Cancel();

        if (isCollided)
        {
            await GameOver(args.Hero);
            return;
        }

        TryIncreaseCherryCount(collect);
        stateMachine.Enter<GameStartState, GameStartState.Arguments>(new(args.CurrentPlatform, args.Hero));
    }
}