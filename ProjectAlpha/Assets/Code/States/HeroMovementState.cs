using System.Threading;
using System.Threading.Tasks;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Code.States;

public abstract class BaseHeroMovementState
{
    private readonly InputManager _inputManager;
    private readonly GameMediator _gameMediator;
    private readonly Camera _camera;
    private readonly CancellationToken _token;

    protected BaseHeroMovementState(InputManager inputManager, GameMediator gameMediator, Camera camera)
    {
        _inputManager = inputManager;
        _gameMediator = gameMediator;
        _camera = camera;
    }

    protected async UniTask HeroFlipsOnClick(IHero hero, IPlatform leftPlatform,
        IPlatform rightPlatform, CancellationToken token)
    {
        await foreach (AsyncUnit _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            if (hero.Intersect(leftPlatform) || hero.Intersect(rightPlatform))
                continue;

            hero.Flip();
        }
    }

    protected static async UniTask<bool> HeroCollides(
        IHero hero, IPlatform nextPlatform, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (hero.Intersect(nextPlatform) && hero.IsFlipped) break;
            await UniTask.Yield(token);
        }

        return true;
    }

    protected static async UniTask HeroCollectsCherry(
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

    protected async UniTask MoveHero(float destinationX, IHero hero, CancellationToken token)
    {
        await UniTask.Delay(200, cancellationToken: token); //move it to another place
        _camera.MoveBackgroundAsync(token).Forget();
        await hero.MoveAsync(destinationX, token);
    }

    protected async Task GameOver(IHero hero)
    {
        await UniTask.Delay(100);
        await hero.FallAsync();
        await UniTask.Delay(300);
        _gameMediator.GameOver();
    }

    protected void TryIncreaseCherryCount(UniTask collectTask)
    {
        if (collectTask.Status == UniTaskStatus.Succeeded)
        {
            _gameMediator.IncreaseCherryCount();
        }
    }
}