using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToPlatformState : MoveHeroBaseState, IState<MoveHeroToPlatformState.Arguments>
{
    private readonly StickSpawner _stickSpawner;
    private readonly IInputManager _inputManager;
    private readonly GameMediator _gameMediator;
    private readonly CancellationToken _token;

    public readonly record struct Arguments(
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        IStick Stick,
        ICherry Cherry);

    public MoveHeroToPlatformState(IInputManager inputManager, GameMediator gameMediator, StickSpawner stickSpawner, Camera camera, ScopeToken token, GameStateMachine.Settings settings) :
        base(gameMediator, camera, settings)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
        _gameMediator = gameMediator;
        _token = token;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        (IPlatform leftPlatform, IPlatform currentPlatform, IHero hero, IStick stick, ICherry cherry) = args;

        CancellationTokenSource cts = new();
        CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _token).Token;

        HeroFlippingOnClick(hero, leftPlatform, currentPlatform, linkedToken).Forget();
        UniTask collecting = CollectingCherry(hero, cherry, linkedToken);
        bool isCollided = await HeroMoving(hero, currentPlatform, linkedToken);
        cts.Cancel();

        if (isCollided)
        {
            await EndGame(hero, stick);
            return;
        }

        UpdateCherries(collecting);
        _gameMediator.IncreaseScore();

        stateMachine.Enter<NextRoundState, NextRoundState.Arguments>(
            new(currentPlatform, hero));
    }

    private async UniTaskVoid HeroFlippingOnClick(IHero hero, IPlatform leftPlatform, IPlatform rightPlatform, CancellationToken token)
    {
        await foreach (AsyncUnit _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            bool isHeroOnAnyPlatform = hero.Intersect(leftPlatform) || hero.Intersect(rightPlatform);
            if (isHeroOnAnyPlatform) continue;
            hero.Flip();
        }
    }

    private async UniTask<bool> HeroMoving(IHero hero, IPlatform currentPlatform, CancellationToken linkedToken)
    {
        float destinationX = currentPlatform.Borders.Right;
        destinationX -= _stickSpawner.StickWidth / 2f;

        (bool isCollided, _) = await UniTask.WhenAny(
            HeroColliding(hero, currentPlatform, linkedToken),
            HeroMoving(destinationX, hero, linkedToken));

        return isCollided;
    }
}
