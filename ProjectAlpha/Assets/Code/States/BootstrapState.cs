﻿using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class BootstrapState : IState
{
    private readonly CameraController _cameraController;
    private readonly GameTriggers _gameTriggers;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        CameraController cameraController,
        GameTriggers gameTriggers)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _cameraController = cameraController;
        _gameTriggers = gameTriggers;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        UniTask loadBackgroundTask = _cameraController.ChangeBackgroundAsync();

        Vector2 platformPosition = _cameraController.ViewportToWorldPosition(new Vector2(0.5f, 0.2f));
        IPlatformController menuPlatform = await _platformSpawner.CreatePlatformAsync(platformPosition, 2f, Relative.Center);

        IHeroController hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Position, Relative.Left);

        await _gameTriggers.StartGameClicked.OnClickAsync();

        await loadBackgroundTask;

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform, hero));

        //set idle animation
    }

    public void Exit() { }
}