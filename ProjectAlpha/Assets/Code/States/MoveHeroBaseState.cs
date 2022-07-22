using System;
using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Code.States;

public abstract class MoveHeroBaseState
{
    private readonly GameMediator _gameMediator;
    private readonly Camera _camera;
    private readonly GameStateMachine.Settings _settings;
    private readonly CancellationToken _token;

    protected MoveHeroBaseState(GameMediator gameMediator, Camera camera, GameStateMachine.Settings settings)
    {
        _gameMediator = gameMediator;
        _camera = camera;
        _settings = settings;
    }

    protected static async UniTask<bool> HeroColliding(
        IHero hero, IPlatform nextPlatform, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (hero.Intersect(nextPlatform) && hero.IsFlipped) break;
            await UniTask.Yield(token);
        }

        return true;
    }

    protected static async UniTask CollectingCherry(
        IHero hero, ICherry cherry, CancellationToken token)
    {
        await foreach (AsyncUnit _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))
        {
            if (hero.Intersect(cherry))
            {
                cherry.PickUp();
                return;
            }
        }
    }

    protected async UniTask HeroMoving(float destinationX, IHero hero, CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_settings.DelayBeforeHeroMovement));

        _camera.MoveBackgroundAsync(token).Forget();
        await hero.MoveAsync(destinationX, token);
    }

    protected async UniTask EndGame(IHero hero, IStick stick)
    {
        stick.RotateDownAsync().Forget();
        await hero.FallAsync(_camera.Borders.Bot - hero.Borders.Height);
        await UniTask.Delay(TimeSpan.FromSeconds(_settings.DelayBeforeEndGame));
        await _camera.Punch(_token);

        _gameMediator.GameOver();
    }

    protected void UpdateCherries(UniTask collectTask)
    {
        bool isCherryCollected = collectTask.Status == UniTaskStatus.Succeeded;
        if (isCherryCollected)
            _gameMediator.AddCherry();
    }
}
