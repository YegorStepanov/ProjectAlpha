﻿using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class MoveHeroToNextPlatformState : IState<MoveHeroToNextPlatformState.Arguments>
{
    private readonly StickSpawner _stickSpawner;

    public readonly record struct Arguments(
        IPlatform CurrentPlatform,
        IPlatform NextPlatform,
        IStick Stick,
        IHero Hero,
        ICherry Cherry);

    public MoveHeroToNextPlatformState(StickSpawner stickSpawner)
    {
        _stickSpawner = stickSpawner;
    }
    
    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        if (args.NextPlatform.Contains(args.Stick.ArrowPosition))
        {
            //it should be created AFTER move, to be able to reuse only 2, not 3
            IStick stick = await _stickSpawner.CreateStickAsync(args.NextPlatform.Borders.RightTop);

            stateMachine.Enter<HeroMovementToPlatformState, HeroMovementToPlatformState.Arguments>(
                new(args.CurrentPlatform, args.NextPlatform, args.Hero, stick, args.Cherry));
        }
        else
        {
            stateMachine.Enter<HeroMovementToGameOverState, HeroMovementToGameOverState.Arguments>(
                new(args.CurrentPlatform, args.NextPlatform, args.Hero, args.Stick, args.Cherry));
        }

        await UniTask.CompletedTask;
    }

    public void Exit() { }
}