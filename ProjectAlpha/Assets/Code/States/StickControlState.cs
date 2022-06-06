﻿using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class StickControlState : IState<StickControlState.Arguments>
{
    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IHero Hero,
        ICherry Cherry);

    private readonly StickSpawner _stickSpawner;
    private readonly InputManager _inputManager;
    private readonly GameMediator _gameMediator;

    public StickControlState(StickSpawner stickSpawner, InputManager inputManager, GameMediator gameMediator)
    {
        _stickSpawner = stickSpawner;
        _inputManager = inputManager;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        IStick stick = await _stickSpawner.CreateAsync(args.CurrentPlatform.Borders.RightTop);

        await _inputManager.NextClick();
        stick.StartIncreasing();

        await _inputManager.NextRelease();
        stick.StopIncreasing();

        await args.Hero.KickAsync();

        await stick.RotateAsync();

        bool isInside = args.NextPlatform.InsideRedPoint(stick.ArrowPosition.x);

        if (isInside)
            _gameMediator.OnRedPointHit(args.NextPlatform.RedPointBorders.Center);

        stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
            new(args.CurrentPlatform, args.NextPlatform, stick, args.Hero, args.Cherry));
    }
}