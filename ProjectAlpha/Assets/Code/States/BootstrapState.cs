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
    private readonly StartSceneInformer _startSceneInformer;
    private readonly WidthGenerator _widthGenerator;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        WidthGenerator widthGenerator,
        HeroSpawner heroSpawner,
        CameraController cameraController,
        GameTriggers gameTriggers,
        StartSceneInformer startSceneInformer)
    {
        _platformSpawner = platformSpawner;
        _widthGenerator = widthGenerator;
        _heroSpawner = heroSpawner;
        _cameraController = cameraController;
        _gameTriggers = gameTriggers;
        _startSceneInformer = startSceneInformer;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        //_widthGenerator.Reset();

        UniTask loadBackgroundTask = _cameraController.ChangeBackgroundAsync();

        Vector2 platformPosition = _cameraController.ViewportToWorldPosition(new Vector2(0.5f, 0.2f));
        IPlatformController menuPlatform = await _platformSpawner.CreatePlatformAsync(platformPosition, 2f, Relative.Center);

        IHeroController hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Position, Relative.Left);

        if (!_startSceneInformer.IsGameStartScene)
            await _gameTriggers.StartGameTrigger.OnClickAsync();

        await loadBackgroundTask;

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform, hero));

        //set background randomly
        //set idle animation

        // hero = Object.Instantiate(heroPrefab, t1.position, Quaternion.identity);
        // Vector2 worldSize = SpriteHelper.WorldSpriteSize(hero.GetChild(0).GetComponent<SpriteRenderer>().sprite, hero.lossyScale);
        //
        // Vector3 pos = hero.position;
        // pos.x += worldSize.x / 2f;
        // hero.position = pos;
    }

    public void Exit() { }
}