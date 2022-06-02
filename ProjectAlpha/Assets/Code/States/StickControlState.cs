using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class StickControlState : IState<StickControlState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IHero Hero,
        ICherry Cherry,
        IStick Stick);

    private readonly InputManager _inputManager;
    private readonly GameMediator _gameMediator;

    public StickControlState(InputManager inputManager, GameMediator gameMediator)
    {
        _inputManager = inputManager;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        IStick stick = args.Stick;

        await _inputManager.NextMouseClick();
        stick.StartIncreasing();

        await _inputManager.NextMouseRelease();
        stick.StopIncreasing();

        await args.Hero.KickAsync();

        await stick.RotateAsync();

        bool isInside = args.NextPlatform.IsInsideRedPoint(stick.ArrowPosition.x);
        Debug.Log("IsInsideRedPoint: " + isInside);

        if (isInside)
            _gameMediator.OnRedPointHit(args.NextPlatform.RedPointBorders.Center);

        stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new(args.CurrentPlatform, args.NextPlatform, stick, args.Hero, args.Cherry));
    }

    public void Exit() { }
}