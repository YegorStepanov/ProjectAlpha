using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatformController CurrentPlatform,
        IPlatformController NextPlatform,
        IStickController Stick,
        IHeroController Hero,
        ICherryController Cherry);

    private readonly GameMediator _gameMediator;

    public MoveHeroToNextPlatformState(GameMediator gameMediator)
    {
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (IsStickOnPlatform(args.Stick, args.NextPlatform))
        {
            stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
                new(args.CurrentPlatform, args.NextPlatform, args.Hero, args.Cherry));
        }
        else
        {
            await args.Hero.MoveAsync(args.Stick.Borders.Right);
            await UniTask.Delay(100);
            await args.Hero.FellAsync();
            await UniTask.Delay(300);

            _gameMediator.GameOver();
        }
    }

    public void Exit() { }

    private static bool IsStickOnPlatform(IStickController stick, IPlatformController platform)
    {
        float stickPosX = stick.Borders.Right;

        if (stickPosX < platform.Borders.Left)
            return false;

        if (stickPosX > platform.Borders.Right)
            return false;

        return true;
    }
}