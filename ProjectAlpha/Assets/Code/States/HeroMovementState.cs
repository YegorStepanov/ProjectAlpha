using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using JetBrains.Annotations;

namespace Code.States;

public sealed class HeroMovementState : IState<HeroMovementState.Arguments>
{
    private readonly InputManager _inputManager;
    private readonly StickSpawner _stickSpawner;
    private readonly GameMediator _gameMediator;

    public readonly record struct Arguments(
        bool IsGameOver,
        IPlatformController LeftPlatform,
        IPlatformController CurrentPlatform,
        IHeroController Hero,
        [CanBeNull] ICherryController Cherry,
        [CanBeNull] IStickController Stick);

    public HeroMovementState(InputManager inputManager, StickSpawner stickSpawner, GameMediator gameMediator)
    {
        _inputManager = inputManager;
        _stickSpawner = stickSpawner;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        using CancellationTokenSource cts = new();

        _ = FlippingHeroOnClickAsync(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token);
        
        if(args.Cherry != null)
            _ = PickingUpCherryByHero(args.Hero, args.Cherry, cts.Token);

        UniTask<bool> checkHeroCollision = CollidingHeroWithPlatform(args.Hero, args.CurrentPlatform, cts.Token);

        var dest = args.IsGameOver && args.Stick != null
            ? args.Stick.Borders.Right
            : args.CurrentPlatform.Borders.Right;

        UniTask move = MoveHeroAsync(dest, args.Hero, cts.Token);

        (bool isHeroCollided, _) = await UniTask.WhenAny(checkHeroCollision, move);
        cts.Cancel();

        if (isHeroCollided || args.IsGameOver)
        {
            await UniTask.Delay(100);
            await args.Hero.FellAsync();
            await UniTask.Delay(300);
            _gameMediator.GameOver();
        }

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(new(args.CurrentPlatform, args.Hero));
    }

    private async UniTask FlippingHeroOnClickAsync(IHeroController hero, IPlatformController leftPlatform,
        IPlatformController rightPlatform, CancellationToken token)
    {
        await foreach (var _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            if (leftPlatform.Borders.Right < hero.Borders.Left && hero.Borders.Right < rightPlatform.Borders.Left)
            {
                hero.Flip();
            }
        }
    }

    private static async UniTask<bool> CollidingHeroWithPlatform(
        IHeroController hero, IPlatformController nextPlatform, CancellationToken token)
    {
        await UniTask.WaitUntil(() => hero.Borders.Right > nextPlatform.Borders.Left && hero.IsFlipped,
            cancellationToken: token);
        return true;
    }

    private static async UniTask PickingUpCherryByHero(
        IHeroController hero, ICherryController cherry, CancellationToken token)
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))
        {
            if (hero.Borders.Intersects(cherry.Borders))
            {
                cherry.PickUp();
                return;
            }
        }
    }

    private async UniTask MoveHeroAsync(float destinationX, IHeroController hero, CancellationToken token)
    {
        float destination = destinationX;
        destination -= _stickSpawner.StickWidth / 2f;
        destination -= hero.HandOffset;
        await UniTask.Delay(200); //move it to another place
        await hero.MoveAsync(destination, token);
    }

    public void Exit() { }
}