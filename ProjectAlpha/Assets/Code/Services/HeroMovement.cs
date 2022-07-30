using System.Threading;
using Code.Common;
using Code.Extensions;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Entities.Stick;
using Code.Services.Infrastructure;
using Code.Settings;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Code.Services;

public class HeroMovement
{
    private readonly IInputManager _inputManager;
    private readonly ICamera _camera1;
    private readonly StickSpawner _stickSpawner;
    private readonly GameSettings _settings;
    private readonly CancellationToken _token;

    public HeroMovement(IInputManager inputManager, ICamera camera1, StickSpawner stickSpawner, GameSettings settings, CancellationToken token)
    {
        _inputManager = inputManager;
        _camera1 = camera1;
        _stickSpawner = stickSpawner;
        _settings = settings;
        _token = token;
    }

    public UniTask<Result> MoveHeroAsync(IHero hero, IPlatform currentPlatform, IPlatform destinationPlatform,
        Destination destination, HeroFlipOption heroFlipOption)
    {
        return MoveHeroAsync(hero, currentPlatform, destinationPlatform, CherryNull.Default, StickNull.Default, destination, heroFlipOption);
    }

    public async UniTask<Result> MoveHeroAsync(IHero hero, IPlatform currentPlatform, IPlatform destinationPlatform, ICherry cherry, IStick stick,
        Destination destination, HeroFlipOption heroFlipOption)
    {
        using LinkedCancellationTokenSourceDisposable cts = new(_token);
        CancellationToken token = cts.Token;

        float destinationX = GetDestinationX(hero, destinationPlatform, stick, destination);
        await Delay(token);
        return await MoveHeroAsync(hero, currentPlatform, destinationPlatform, cherry, destinationX, heroFlipOption, token);
    }

    private float GetDestinationX(IHero hero, IPlatform destinationPlatform, IStick stick, Destination destination) =>
        destination == Destination.PlatformEnd
            ? destinationPlatform.Borders.Right - _stickSpawner.StickWidth
            : stick.Borders.Right + hero.Borders.HalfWidth;

    private UniTask Delay(CancellationToken token) =>
        UniTask.Delay(_settings.DelayBeforeHeroMovement, cancellationToken: token);

    private async UniTask<Result> MoveHeroAsync(IHero hero, IPlatform currentPlatform, IPlatform destinationPlatform, ICherry cherry,
        float destinationX, HeroFlipOption heroFlipOption, CancellationToken token)
    {
        MoveBackground(token);
        FlipHeroOnClick(hero, currentPlatform, destinationPlatform, heroFlipOption, token);
        UniTask pickCherry = PickCherry(hero, cherry, token);

        UniTask checkCollidingHero = CheckCollidingHero(hero, destinationPlatform, token);
        UniTask moveHero = hero.MoveAsync(destinationX, token);
        int index = await UniTask.WhenAny(checkCollidingHero, moveHero);

        return new Result(IsHeroCollided(), IsCherryCollected(pickCherry));

        bool IsHeroCollided() => index == 0;
        static bool IsCherryCollected(UniTask pickCherry) => pickCherry.Status == UniTaskStatus.Succeeded;
    }

    private void MoveBackground(CancellationToken token) =>
        _camera1.MoveBackgroundAsync(token).Forget();

    private static async UniTask CheckCollidingHero(IHero hero, IPlatform platform, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (hero.Intersect(platform) && hero.IsFlipped)
                return;

            await UniTask.Yield(token);
        }
    }

    private void FlipHeroOnClick(IHero hero, IPlatform leftPlatform, IPlatform rightPlatform, HeroFlipOption strategy, CancellationToken token) =>
        FlipHeroOnClickImpl(hero, leftPlatform, rightPlatform, strategy, token).Forget();

    private async UniTaskVoid FlipHeroOnClickImpl(IHero hero, IPlatform leftPlatform, IPlatform rightPlatform, HeroFlipOption strategy, CancellationToken token)
    {
        await foreach (AsyncUnit _ in _inputManager.OnClickAsAsyncEnumerable().WithCancellation(token))
        {
            bool canBeFlipped = strategy == HeroFlipOption.AllowFlippingOnDestinationPlatform
                ? !hero.Intersect(leftPlatform)
                : !hero.Intersect(leftPlatform) && !hero.Intersect(rightPlatform);

            if (canBeFlipped)
                hero.Flip();
        }
    }

    private static async UniTask PickCherry(IHero hero, ICherry cherry, CancellationToken token)
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

    public readonly record struct Result(bool IsHeroCollided, bool IsCherryCollected);

    public enum Destination
    {
        StickEnd,
        PlatformEnd,
    }

    public enum HeroFlipOption
    {
        DisallowFlippingOnDestinationPlatform,
        AllowFlippingOnDestinationPlatform
    }
}
