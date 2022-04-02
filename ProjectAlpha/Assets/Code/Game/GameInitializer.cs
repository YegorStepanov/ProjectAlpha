using Code.Game.States;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public sealed class GameInitializer : IInitializable
    {
        private readonly GameStateMachine gameStateMachine;

        public GameInitializer(GameStateMachine gameStateMachine) => 
            this.gameStateMachine = gameStateMachine;

        public void Initialize()
        {
            Debug.Log("GameInitializer.Initialize");
     
            gameStateMachine.Enter<BootstrapState>();
        }
    }
}