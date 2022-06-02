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
    private readonly GameMediator _gameMediator;

    public readonly record struct Arguments(
        bool IsGameOver,
        IPlatform LeftPlatform,
        IPlatform CurrentPlatform,
        IHero Hero,
        [CanBeNull] ICherry Cherry,
        IStick Stick);

    public HeroMovementState(InputManager inputManager, GameMediator gameMediator)
    {
        _inputManager = inputManager;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        using CancellationTokenSource cts = new();

        _ = FlippingHeroOnClick(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token);
        
        if(args.Cherry != null)
            _ = PickingUpCherryByHero(args.Hero, args.Cherry, cts.Token);

        UniTask<bool> checkHeroCollision = CollidingHeroWithPlatform(args.Hero, args.CurrentPlatform, cts.Token);

        var dest = args.IsGameOver
            ? args.Stick.Borders.Right
            : args.CurrentPlatform.Borders.Right;

        UniTask move = MoveHeroAsync(dest, args.Hero, args.Stick, cts.Token);

        (bool isHeroCollided, _) = await UniTask.WhenAny(checkHeroCollision, move);
        cts.Cancel();

        if (isHeroCollided || args.IsGameOver)
        {
            await UniTask.Delay(100);
            await args.Hero.FellAsync();
            await UniTask.Delay(300);
            _gameMediator.GameOver();
        }

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(new(args.CurrentPlatform, args.Hero, args.Stick));
    }

    private async UniTask FlippingHeroOnClick(IHero hero, IPlatform leftPlatform,
        IPlatform rightPlatform, CancellationToken token)
    {
        await foreach (var _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            if (!hero.Intersect(leftPlatform) && !hero.Intersect(rightPlatform))
            {
                hero.Flip();
            }
        }
    }

    private static async UniTask<bool> CollidingHeroWithPlatform(
        IHero hero, IPlatform nextPlatform, CancellationToken token)
    {
        await UniTask.WaitUntil(() => hero.Intersect(nextPlatform) && hero.IsFlipped,
            cancellationToken: token);
        return true;
    }

    private static async UniTask PickingUpCherryByHero(
        IHero hero, ICherry cherry, CancellationToken token)
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))
        {
            if (hero.Intersect(cherry))
            {
                cherry.PickUp();
                return;
            }
        }
    }

    private async UniTask MoveHeroAsync(float destinationX, IHero hero, IStick stick, CancellationToken token)
    {
        float destination = destinationX;
        //do it only for platform, not for endgame
        destination -= stick.Borders.HalfWidth;
        destination -= hero.HandOffset;
        await UniTask.Delay(200); //move it to another place
        await hero.MoveAsync(destination, token);
    }

    public void Exit() { }
}