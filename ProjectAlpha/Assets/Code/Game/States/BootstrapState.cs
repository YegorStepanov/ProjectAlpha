using Code.Common;
using Code.Project;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.States
{
    public sealed class BootstrapState : IState
    {
        private readonly GameStateMachine stateMachine;
        private readonly PlatformSpawner platformSpawner;
        private readonly WidthGenerator widthGenerator;
        private readonly IHeroController hero;
        private readonly CameraService cameraService;
        private readonly GameTriggers gameTriggers;

        public BootstrapState(
            GameStateMachine stateMachine,
            PlatformSpawner platformSpawner,
            WidthGenerator widthGenerator,
            IHeroController hero,
            CameraService cameraService, GameTriggers gameTriggers)
        {
            this.stateMachine = stateMachine;
            this.platformSpawner = platformSpawner;
            this.widthGenerator = widthGenerator;
            this.hero = hero;
            this.cameraService = cameraService;
            this.gameTriggers = gameTriggers;
        }

        public class Settings
        {
            public float MenuPlatformWidth = 2f;

            public float MenuPlatformViewportPosX = 0.5f; //= new(0.5f, 0.2f);
            // public float GamePlatformViewportPosX = 1f; //= new(1f, 0.3f); //y should be synced with camera

            public float MenuViewportPosY = 0.2f;
            public float GameViewportPosY = 0.3f;
        }

        public async UniTaskVoid EnterAsync()
        {
            Debug.Log("BootstrapState.Enter" + ": " + Time.frameCount);
            widthGenerator.Reset();

            Vector2 platformPosition = cameraService.ViewportToWorldPosition(new Vector2(0.5f, 0.2f));
            IPlatformController menuPlatform = platformSpawner.CreatePlatform(platformPosition, 2f, Relative.Center);

            hero.TeleportTo(menuPlatform.Position, Relative.Left);

            if(gameTriggers.StartGameButtonTrigger.IsTriggerExist)
               await gameTriggers.StartGameButtonTrigger.OnClickAsync();

            stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                new GameStartState.Arguments(menuPlatform));

            //set background randomly
            //set idle animation

            // hero = Object.Instantiate(heroPrefab, t1.position, Quaternion.identity);
            // Vector2 worldSize = SpriteHelper.WorldSpriteSize(hero.GetChild(0).GetComponent<SpriteRenderer>().sprite, hero.lossyScale);
            //
            // Vector3 pos = hero.position;
            // pos.x += worldSize.x / 2f;
            // hero.position = pos;
        }

        private async UniTaskVoid ExitAsync()
        {
            //hide ui
            //close ui scene
            //show game ui

            await UniTask.CompletedTask;

            //Show your finder 
            //after next platform destination start fade in it 
        }

        public void Exit() { }
    }
}