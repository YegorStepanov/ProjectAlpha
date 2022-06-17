using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class HeroMovementToPlatformState : BaseHeroMovementState, IState<HeroMovementToPlatformState.Arguments>
{
    private readonly StickSpawner _stickSpawner;
    private readonly CancellationToken _token;

    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        ICherry Cherry);

    public HeroMovementToPlatformState(
        InputManager inputManager, GameMediator gameMediator, StickSpawner stickSpawner, Camera camera, ScopeCancellationToken token) :
        base(inputManager, gameMediator, camera)
    {
        _stickSpawner = stickSpawner;
        _token = token.Token;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();
        CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _token).Token;

        HeroFlipsOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, linkedToken).Forget();

        UniTask collect = HeroCollectsCherry(args.Hero, args.Cherry, linkedToken); //todo

        float destinationX = args.CurrentPlatform.Borders.Right;
        destinationX -= _stickSpawner.StickWidth / 2f;
        destinationX -= args.Hero.HandOffset;

        (bool isCollided, _) = await UniTask.WhenAny(
            HeroCollides(args.Hero, args.CurrentPlatform, linkedToken),
            MoveHero(destinationX, args.Hero, linkedToken));

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