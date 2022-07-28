using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToMenuPlatformState : IState<MoveHeroToMenuPlatformState.Arguments>
{
    private readonly HeroMovement _heroMovement;

    public readonly record struct Arguments(IHero Hero, IPlatform MenuPlatform);

    public MoveHeroToMenuPlatformState(HeroMovement heroMovement) => 
        _heroMovement = heroMovement;

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        (IHero hero, IPlatform menuPlatform) = args;

        await _heroMovement.MoveHeroAsync(
            hero, menuPlatform, menuPlatform,
            HeroMovement.Destination.PlatformEnd, HeroMovement.HeroFlipOption.DisallowFlippingOnDestinationPlatform);

        stateMachine.Enter<NextRoundState, NextRoundState.Arguments>(
            new(menuPlatform, hero));
    }
}
