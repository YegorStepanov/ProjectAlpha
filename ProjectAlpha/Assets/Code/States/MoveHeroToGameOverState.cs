using Code.Services;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToGameOverState : IState<MoveHeroToGameOverState.Arguments>
{
    public readonly record struct Arguments(IPlatform LeftPlatform, IPlatform CurrentPlatform, IHero Hero, IStick Stick, ICherry Cherry);

    private readonly HeroMovement _heroMovement;

    public MoveHeroToGameOverState(HeroMovement heroMovement) =>
        _heroMovement = heroMovement;

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        await _heroMovement.MoveHeroAsync(
            args.Hero, args.LeftPlatform, args.CurrentPlatform, args.Cherry, args.Stick,
            HeroMovement.Destination.StickEnd, HeroMovement.HeroFlipOption.AllowFlippingOnDestinationPlatform);

        await UniTask.Delay(100); //todo:

        stateMachine.Enter<EndGameState, EndGameState.Arguments>(
            new(args.Hero, args.Stick));
    }
}
