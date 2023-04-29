using Code.Services;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.States
{
    public sealed class StickControlState : IState<GameData>
    {
        private readonly StickSpawner _stickSpawner;
        private readonly IInputManager _inputManager;
        private readonly GameUIController _gameUIController;

        public StickControlState(
            StickSpawner stickSpawner,
            IInputManager inputManager,
            GameUIController gameUIController)
        {
            _stickSpawner = stickSpawner;
            _inputManager = inputManager;
            _gameUIController = gameUIController;
        }

        public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            IStick nextStick = await _stickSpawner.CreateAsync(data.CurrentPlatform.Borders.RightTop);
            await ControlStick(nextStick, data.Hero, data.NextPlatform);

            GameData nextData = data with { Stick = nextStick };

            if (nextStick.IsStickArrowOn(data.NextPlatform))
                stateMachine.Enter<HeroMovementToPlatformState, GameData>(nextData);
            else
                stateMachine.Enter<HeroMovementToEndGameState, GameData>(nextData);
        }

        private async UniTask<IStick> ControlStick(IStick stick, IHero hero, IPlatform nextPlatform)
        {
            await IncreaseStick(stick, hero);
            await hero.KickAsync();
            await stick.RotateAsync();
            HandleRedPointHit(stick, nextPlatform);
            return stick;
        }

        private async UniTask IncreaseStick(IStick stick, IHero hero)
        {
            await _inputManager.WaitClick();
            using CancellationTokenDisposable cts = new();

            hero.Squatting(cts.Token);
            stick.Increasing(cts.Token);

            await _inputManager.WaitClickRelease();
        }

        private void HandleRedPointHit(IStick stick, IPlatform platform)
        {
            if (stick.IsStickArrowOn(platform.PlatformRedPoint))
                _gameUIController.HitRedPoint(platform.PlatformRedPoint.Borders.Center);
        }
    }
}