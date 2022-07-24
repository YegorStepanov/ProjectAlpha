using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToGameOverState : MoveHeroBaseState, IState<MoveHeroToGameOverState.Arguments>
{
    public readonly record struct Arguments(IPlatform LeftPlatform, IPlatform CurrentPlatform, IHero Hero, IStick Stick, ICherry Cherry);

    private readonly CancellationToken _token;
    private readonly IInputManager _inputManager;

    public MoveHeroToGameOverState(IInputManager inputManager, GameMediator gameMediator, Camera camera, GameStateMachine.Settings settings, CancellationToken token)
        : base(gameMediator, camera, settings)
    {
        _inputManager = inputManager;
        _token = token;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        CancellationTokenSource cts = new();
        CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _token).Token;

        HeroFlippingOnClick(args.Hero, args.LeftPlatform, linkedToken).Forget();
        UniTask collecting = CollectingCherry(args.Hero, args.Cherry, linkedToken);
        await HeroMoving(args, linkedToken);
        cts.Cancel();
        UpdateCherries(collecting);
        //GameOverState
        await EndGame(args.Hero, args.Stick);
    }

    private async UniTaskVoid HeroFlippingOnClick(IHero hero, IPlatform leftPlatform, CancellationToken token)
    {
        await foreach (AsyncUnit _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            //The user can flip over and hit the right platform for faster loss
            if (hero.Intersect(leftPlatform)) continue;
            hero.Flip();
        }
    }

    private UniTask HeroMoving(Arguments args, CancellationToken linkedToken)
    {
        float destinationX = args.Stick.Borders.Right;
        return UniTask.WhenAny(
            HeroColliding(args.Hero, args.CurrentPlatform, linkedToken),
            HeroMoving(destinationX, args.Hero, linkedToken));
    }
}
