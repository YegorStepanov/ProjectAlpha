﻿using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class BootstrapState : IState
{
    private readonly Camera _camera;
    private readonly GameEvents _gameEvents;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        Camera camera,
        GameEvents gameEvents)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _camera = camera;
        _gameEvents = gameEvents;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        _gameData.SetMenuHeight();

        await _camera.ChangeBackgroundAsync();

        IPlatform menuPlatform = await _platformSpawner.CreateMenuAsync();

        IHero hero = await _heroSpawner.CreateAsync(menuPlatform.Borders.CenterTop, Relative.Left);

        await _gameEvents.GameStarted.WaitAsync();

        stateMachine.Enter<HeroMovementToPlatformState, HeroMovementToPlatformState.Arguments>(
            new(menuPlatform, menuPlatform, hero, new CherryNull()));
    }
}