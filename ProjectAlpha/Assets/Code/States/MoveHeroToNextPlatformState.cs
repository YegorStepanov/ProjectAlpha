using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IArgState<MoveHeroToNextPlatformState.Arguments>
{
    public readonly record struct Arguments(
        IPlatformController CurrentPlatform,
        IPlatformController NextPlatform,
        IStickController Stick, IHeroController Hero);

    private readonly PlatformSpawner _platformSpawner;
    private readonly GameUIMediator _gameUIMediator;

    public MoveHeroToNextPlatformState(PlatformSpawner platformSpawner, GameUIMediator gameUIMediator)
    {
        _platformSpawner = platformSpawner;
        _gameUIMediator = gameUIMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (IsStickOnPlatform(args.Stick, args.NextPlatform))
        {
            stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                new GameStartState.Arguments(args.CurrentPlatform, args.NextPlatform, args.Hero));
        }
        else
        {
            await args.Hero.MoveAsync(args.Stick.Borders.Right);
            await UniTask.Delay(100);
            await args.Hero.FellAsync();
            await UniTask.Delay(300);

            _gameUIMediator.ShowGameOver();
            //earthshake screen
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