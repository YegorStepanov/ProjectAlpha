using System;
using System.Collections.Generic;
using Code.Game.States;
using Zenject;

namespace Code.Game
{
    // public sealed class GameStateMachineInitializer : MonoBehaviour
    // {
    //     private readonly GameStateMachine gameStateMachine;
    //
    //     private async UniTaskVoid Awake()
    //     {
    //         // gameStateMachine.Enter<BootstrapState>();
    //     }
    // }


    public sealed class GameStateMachine
    {
        private Dictionary<Type, IExitState> states;
        private IExitState activeState;

        [Inject]
        public void Construct(DiContainer container) => states = new Dictionary<Type, IExitState>
        {
            [typeof(BootstrapState)] = container.Instantiate<BootstrapState>(),
            [typeof(GameStartState)] = container.Instantiate<GameStartState>(),
            [typeof(StickControlState)] = container.Instantiate<StickControlState>(),
            [typeof(MoveHeroToNextPlatformState)] = container.Instantiate<MoveHeroToNextPlatformState>(),
        };

        public void Enter<TState>() where TState : class, IState =>
            ChangeState<TState>().EnterAsync().Forget();

        public void Enter<TState, TArg>(TArg argument) where TState : class, IArgState<TArg> => //hmm
            ChangeState<TState>().EnterAsync(argument).Forget();

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            activeState?.Exit();

            var state = GetState<TState>();
            activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState =>
            states[typeof(TState)] as TState;
    }
}