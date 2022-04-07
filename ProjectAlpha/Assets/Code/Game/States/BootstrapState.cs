using Code.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Game.States
{
    public sealed class BootstrapState : IState
    {
        private readonly GameStateMachine stateMachine;
        private readonly PlatformSpawner platformSpawner;
        private readonly PlatformWidthGenerator platformWidthGenerator;
        private readonly IHeroController hero;
        private readonly CameraService cameraService;

        public BootstrapState(
            GameStateMachine stateMachine,
            PlatformSpawner platformSpawner,
            PlatformWidthGenerator platformWidthGenerator,
            IHeroController hero,
            CameraService cameraService)
        {
            this.stateMachine = stateMachine;
            this.platformSpawner = platformSpawner;
            this.platformWidthGenerator = platformWidthGenerator;
            this.hero = hero;
            this.cameraService = cameraService;
        }

        public void Enter()
        {
            Debug.Log("BootstrapState.Enter" + ": " + Time.frameCount);
            platformWidthGenerator.Reset();

            IPlatformController menuPlatform = platformSpawner.CreateMenuPlatform();
            IPlatformController gamePlatform = platformSpawner.CreatePlatform();

            hero.TeleportTo(menuPlatform.Position, HeroPivot.BottomCenter);


            NewFunction().Forget();

            async UniTaskVoid NewFunction()
            {
                await UniTask.Delay(4000);
                stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                    new GameStartState.Arguments(menuPlatform, gamePlatform));
            }

            //set background randomly
            //set idle animation

            // hero = Object.Instantiate(heroPrefab, t1.position, Quaternion.identity);
            // Vector2 worldSize = SpriteHelper.WorldSpriteSize(hero.GetChild(0).GetComponent<SpriteRenderer>().sprite, hero.lossyScale);
            //
            // Vector3 pos = hero.position;
            // pos.x += worldSize.x / 2f;
            // hero.position = pos;
        }

        public void Exit()
        {
            //hide ui
            //close ui scene
            //show game ui


            //Show your finder 
            //after next platform destination start fade in it 
        }

        // public class Factory : PlaceholderFactory<BootstrapState> { }
    }
}