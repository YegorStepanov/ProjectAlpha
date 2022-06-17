using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class HeroMovementToGameOverState : BaseHeroMovementState, IState<HeroMovementToGameOverState.Arguments>
{
    private readonly CancellationToken _token;

    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        IStick Stick,
        ICherry Cherry);

    public HeroMovementToGameOverState(InputManager inputManager, GameMediator gameMediator, Camera camera, ScopeCancellationToken token) :
        base(inputManager, gameMediator, camera)
    {
        _token = token.Token;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();
        CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _token).Token;

        _ = HeroFlipsOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, linkedToken);
        UniTask collect = HeroCollectsCherry(args.Hero, args.Cherry, linkedToken);

        float destinationX = args.Stick.Borders.Right;

        await UniTask.WhenAny(
            HeroCollides(args.Hero, args.CurrentPlatform, linkedToken),
            MoveHero(destinationX, args.Hero, linkedToken));

        cts.Cancel();

        TryIncreaseCherryCount(collect);

        await GameOver(args.Hero);
    }
}