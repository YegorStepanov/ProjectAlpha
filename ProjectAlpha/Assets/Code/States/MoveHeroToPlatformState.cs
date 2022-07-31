using Code.Services;
using Code.Services.Data;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToPlatformState : IState<MoveHeroToPlatformState.Arguments>
{
    private readonly HeroMovement _heroMovement;
    private readonly IProgress _progress;
    private readonly GameWorld _gameWorld;
    private readonly GameSettings _settings;

    //rename Destination to NextPlatform?
    public readonly record struct Arguments(IPlatform CurrentPlatform, IPlatform DestinationPlatform, IHero Hero, IStick Stick, ICherry Cherry);

    public MoveHeroToPlatformState(HeroMovement heroMovement, IProgress progress, GameWorld gameWorld, GameSettings settings)
    {
        _heroMovement = heroMovement;
        _progress = progress;
        _gameWorld = gameWorld;
        _settings = settings;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        (_, IPlatform destinationPlatform, IHero hero, IStick stick, _) = args;

        HeroMovement.Result result = await _heroMovement.MoveHeroAsync(
            args.Hero, args.CurrentPlatform, args.DestinationPlatform, args.Cherry, args.Stick,
            HeroMovement.Destination.PlatformEnd, HeroMovement.HeroFlipOption.DisallowFlippingOnDestinationPlatform);

        if (result.IsHeroCollided)
        {
            stateMachine.Enter<EndGameState, EndGameState.Arguments>(
                new(hero, stick));

            return;
        }

        //collect cherry only if successful
        if (result.IsCherryCollected)
            _progress.Persistant.AddCherry();

        _progress.Session.IncreaseScore();
        SwitchToGameHeight();

        await UniTask.Delay(_settings.DelayBeforeNextRound);

        stateMachine.Enter<NextRoundState, NextRoundState.Arguments>(
            new(destinationPlatform, hero));
    }

    private void SwitchToGameHeight() =>
        _gameWorld.SwitchToGameHeight();
}
