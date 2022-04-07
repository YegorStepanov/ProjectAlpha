using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Game.States
{
    public sealed class GameStartState : IArgState<GameStartState.Arguments>
    {
        public sealed record Arguments(IPlatformController MenuPlatform, IPlatformController GamePlatform);

        private readonly CameraService cameraService;
        private readonly IHeroController hero;
        private readonly PlatformSpawner platformSpawner;

        public GameStartState(CameraService cameraService, IHeroController hero, PlatformSpawner platformSpawner)
        {
            this.cameraService = cameraService;
            this.hero = hero;
            this.platformSpawner = platformSpawner;
        }
        
        public void Enter(Arguments args)
        {
            var r = args.MenuPlatform.Borders.Left;

            float left = cameraService.MenuBorders.Left;

            var deltaY = args.GamePlatform.Borders.Top - args.MenuPlatform.Borders.Top;

            var delta = new Vector2(left, deltaY);
            
            hero.MoveToAsync(delta).Forget();

            // Transform menuPlatform;
            // heroController.MoveTo(menuPlatform.position);
            //create platform
            //move it randomly
        }
        public void Exit() { }

        // public class Factory : PlaceholderFactory<GameStartState> { }
    }

    public sealed class StickBuildingState
    {
        //hero is присидает каждый тик
        //палка увеличивается линейно

        //next state
        //бьет ногой
        //палка поворачивается
        //

        //чел бежит к концу следующей платформы
        //задний фон плывет

        //пододвинуть камеру к началу новой платформы
    }
}