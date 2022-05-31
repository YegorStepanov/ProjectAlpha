using System;
using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class GameStartState : IArgState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatformController LeftPlatform, IPlatformController CurrentPlatform,
        IHeroController Hero);

    private readonly CameraController _cameraController;
    private readonly PlatformSpawner _platformSpawner;
    private readonly StickSpawner _stickSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly NextPositionGenerator _nextPositionGenerator;
    private readonly GameUIMediator _gameUI;
    private readonly InputManager _inputManager;

    public GameStartState(
        CameraController cameraController,
        PlatformSpawner platformSpawner,
        StickSpawner stickSpawner,
        CherrySpawner cherrySpawner,
        NextPositionGenerator nextPositionGenerator,
        GameUIMediator gameUI,
        InputManager inputManager)
    {
        _cameraController = cameraController;
        _platformSpawner = platformSpawner;
        _stickSpawner = stickSpawner;
        _cherrySpawner = cherrySpawner;
        _nextPositionGenerator = nextPositionGenerator;
        _gameUI = gameUI;
        _inputManager = inputManager;
    }

    private async UniTask WaitClicksAsync(
        IHeroController hero, float minPosition, float maxPosition, CancellationToken token)
    {
        while (true)
        {
            await _inputManager.NextMouseClick(token);

            if (token.IsCancellationRequested) return;

            if (hero.Borders.Left < minPosition)
                Debug.Log("You can't flip");
            else if (hero.Borders.Right > maxPosition)
                Debug.Log("You can't flip 2");
            else
                hero.Flip();
        }
    }

    private async UniTask<bool> CheckIsHeroUnderPlatform(
        IPlatformController nextPlatform,
        IHeroController hero,
        CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (hero.Borders.Right > nextPlatform.Borders.Left && hero.IsFlipped)
            {
                Debug.Log("Bump " + nextPlatform.Borders.Left + " " + hero.Borders.Right);
                return false;
            }

            await UniTask.NextFrame(token);
        }

        return true;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        //using 
        using CancellationTokenSource cts = new();
        _ = WaitClicksAsync(args.Hero, args.LeftPlatform.Borders.Right, args.CurrentPlatform.Borders.Left, cts.Token);
        var check = CheckIsHeroUnderPlatform(args.CurrentPlatform, args.Hero, cts.Token);
        var move = MoveHeroAsync(args.CurrentPlatform, args.Hero, cts.Token);

        var id = await UniTask.WhenAny(check, move);
        cts.Cancel();

        if (id.hasResultLeft)
        {
            throw new Exception("The end");
        }

        _gameUI.IncreaseScore();
        _gameUI.IncreaseCherryCount();

        await UniTask.Delay(100);

        Vector2 destination = new(args.CurrentPlatform.Borders.Left, args.CurrentPlatform.Borders.Bottom);
        var cameraDestination = destination.Shift(_cameraController.Borders, Relative.LeftBottom);
        var platformDestination = _cameraController.Borders.Right +
                                  (args.CurrentPlatform.Borders.Left - _cameraController.Borders.Left); //rework
        
        var moveCameraTask = _cameraController.MoveAsync(cameraDestination);

        _ = args.CurrentPlatform.FadeOutRedPointAsync();

        Vector2 nextPosition = new(
            args.CurrentPlatform.Borders.Left + _cameraController.Borders.Width,
            args.CurrentPlatform.Borders.Top);

        IPlatformController nextPlatform = await _platformSpawner.CreateNextPlatformAsync(nextPosition);
        ICherryController cherry = await _cherrySpawner.CreateCherryAsync(args.CurrentPlatform, nextPlatform);

        await MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform, cherry, platformDestination);

        await moveCameraTask;

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new StickControlState.Arguments(args.CurrentPlatform, nextPlatform, args.Hero));
    }

    private async UniTask MoveNextPlatformToRandomPoint(
        IPlatformController currentPlatform,
        IPlatformController nextPlatform,
        ICherryController cherry,
        float cameraDestination)
    {
        float newPos = _nextPositionGenerator.NextPosition(currentPlatform, nextPlatform, cameraDestination);

        UniTask movePlatform = nextPlatform.MoveAsync(newPos);
        UniTask moveCherry = cherry.MoveRandomlyAsync(currentPlatform, newPos);

        await (movePlatform, moveCherry);
    }

    public void Exit() { }

    private async UniTask MoveHeroAsync(IPlatformController currentPlatform, IHeroController hero,
        CancellationToken token)
    {
        float destX = currentPlatform.Borders.Right;
        destX -= _stickSpawner.StickWidth / 2f;
        destX -= hero.HandOffset;
        await UniTask.Delay(200); //move it to another place
        await hero.MoveAsync(destX, token);
    }
}

//hero is присидает каждый тик

//чел бежит к концу следующей платформы
//задний фон плывет