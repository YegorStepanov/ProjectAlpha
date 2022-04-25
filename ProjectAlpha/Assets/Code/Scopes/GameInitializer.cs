using Code.Services;
using Code.States;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Scopes
{
    public sealed class GameInitializer : IInitializable
    {
        private readonly GameStateMachine gameStateMachine;

        public GameInitializer(GameStateMachine gameStateMachine) =>
            this.gameStateMachine = gameStateMachine;

        public void Initialize()
        {

            if (IsGameStartScene())
            {
                Debug.Log("LOGOGOGO");
            }

            gameStateMachine.Enter<BootstrapState>();
            
            
        }

        private bool IsGameStartScene()
        {
#if UNITY_EDITOR
            return SceneManager.sceneCount == 1 && SceneManager.GetActiveScene().name == SceneAddress.Game.Key;
#else
            return false;
#endif
        }
    }
}