using System;
using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using JetBrains.Annotations;

namespace Code.States;

public sealed class HeroMovementState : IState<HeroMovementState.Arguments>
{
    private readonly InputManager _inputManager;
    private readonly StickSpawner _stickSpawner;

    public readonly record struct Arguments(
        IPlatformController LeftPlatform,
        IPlatformController CurrentPlatform,
        IHeroController Hero,
        [CanBeNull] ICherryController Cherry);

    public HeroMovementState(InputManager inputManager, StickSpawner stickSpawner)
    {
        _inputManager = inputManager;
        _stickSpawner = stickSpawner;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        using CancellationTokenSource cts = new();

        _ = FlippingHeroOnClickAsync(args.Hero, args.LeftPlatform, args.CurrentPlatform, cts.Token);
        _ = PickingUpCherryByHero(args.Hero, args.Cherry, cts.Token);

        UniTask<bool> checkHeroCollision = CollidingHeroWithPlatform(args.Hero, args.CurrentPlatform, cts.Token);
        UniTask move = MoveHeroAsync(args.Hero, args.CurrentPlatform, cts.Token);

        (bool isHeroCollided, _) = await UniTask.WhenAny(checkHeroCollision, move);
        cts.Cancel();

        if (isHeroCollided)
        {
            throw new Exception("The end");
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
            if(hero.Borders.Intersects(cherry.Borders))
            {
                cherry.PickUp();
                return;
            }
        }
    }
    

    private async UniTask MoveHeroAsync(IHeroController hero, IPlatformController currentPlatform,
        CancellationToken token)
    {
        float destX = currentPlatform.Borders.Right;
        destX -= _stickSpawner.StickWidth / 2f;
        destX -= hero.HandOffset;
        await UniTask.Delay(200); //move it to another place
        await hero.MoveAsync(destX, token);
    }

    public void Exit() { }
}