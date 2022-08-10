using System.Threading;
using Code.Common;
using Code.Extensions;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Code.Services;

public class HeroMovement
{
    private readonly IInputManager _inputManager;
    private readonly CameraBackground _cameraBackground;
    private readonly StickSpawner _stickSpawner;
    private readonly GameSettings _settings;
    private readonly CancellationToken _token;

    public HeroMovement(IInputManager inputManager, CameraBackground cameraBackground, StickSpawner stickSpawner, GameSettings settings, CancellationToken token)
    {
        _inputManager = inputManager;
        _cameraBackground = cameraBackground;
        _stickSpawner = stickSpawner;
        _settings = settings;
        _token = token;
    }

    public UniTask<(bool IsHeroCollided, bool IsCherryCollected)> MoveHeroToNextPlatformAsync(
        GameData data, bool canHeroFlipsOnNextPlatform)
    {
        float destinationX = data.NextPlatform.Borders.Right - _stickSpawner.StickWidth - data.Hero.Borders.HalfWidth;
        return MoveHero(destinationX, data, canHeroFlipsOnNextPlatform);
    }

    public UniTask<(bool IsHeroCollided, bool IsCherryCollected)> MoveHeroToStickEndAsync(GameData data, bool canHeroFlipsOnNextPlatform)
    {
        float destinationX = data.Stick.Borders.Right + data.Hero.Borders.HalfWidth;
        return MoveHero(destinationX, data, canHeroFlipsOnNextPlatform);
    }

    private async UniTask<(bool IsHeroCollided, bool IsCherryCollected)> MoveHero(float destinationX, GameData data, bool canHeroFlipsOnNextPlatform)
    {
        await UniTask.Delay(_settings.DelayBeforeHeroMovement, cancellationToken: _token);

        using LinkedCancellationTokenSourceDisposable cts = new(_token);
        CancellationToken stopToken = cts.Token;

        (bool isHeroCollided, bool isCherryCollected) =
            await MoveHero(destinationX, data.Hero, data.CurrentPlatform, data.NextPlatform, data.Cherry, canHeroFlipsOnNextPlatform, stopToken);

        return (isHeroCollided, isCherryCollected);
    }

    private async UniTask<Result> MoveHero(
        float destinationX, IHero hero, IPlatform currentPlatform, IPlatform nextPlatform, ICherry cherry,
        bool canHeroFlipsOnNextPlatform, CancellationToken stopToken)
    {
        MoveBackground(stopToken);
        FlippingHeroOnClick(hero, currentPlatform, nextPlatform, canHeroFlipsOnNextPlatform, stopToken);
        UniTask pickingCherry = PickingCherry(hero, cherry, stopToken);

        int index = await UniTask.WhenAny(
            CheckingIsHeroCollided(hero, nextPlatform, stopToken),
            hero.MoveAsync(destinationX, stopToken));

        return new Result(
            IsHeroCollided: index == 0,
            IsCherryCollected: pickingCherry.Status == UniTaskStatus.Succeeded);
    }

    private void MoveBackground(CancellationToken stopToken) =>
        _cameraBackground.MoveBackgroundAsync(stopToken).Forget();

    private static async UniTask CheckingIsHeroCollided(IHero hero, IPlatform nextPlatform, CancellationToken stopToken)
    {
        while (!stopToken.IsCancellationRequested)
        {
            if (hero.Intersect(nextPlatform) && hero.IsFlipped)
                return;

            await UniTask.Yield(stopToken);
        }
    }

    private void FlippingHeroOnClick(IHero hero, IPlatform currentPlatform, IPlatform nextPlatform, bool canHeroFlipsOnNextPlatform, CancellationToken stopToken)
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            await foreach (AsyncUnit _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(stopToken))
            {
                bool canBeFlipped = canHeroFlipsOnNextPlatform
                    ? !hero.Intersect(currentPlatform)
                    : !hero.Intersect(currentPlatform) && !hero.Intersect(nextPlatform);

                if (canBeFlipped)
                    hero.Flip();
            }
        }
    }

    private static async UniTask PickingCherry(IHero hero, ICherry cherry, CancellationToken token)
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

    private readonly record struct Result(bool IsHeroCollided, bool IsCherryCollected);
}
