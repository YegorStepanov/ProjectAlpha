using System;
using System.Collections.Generic;
using Code.Game.States;
using Zenject;

namespace Code.Game
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitState> states;
        private IExitState activeState;

        public GameStateMachine(
            BootstrapState.Factory boostrapStateFactory,
            PendingStartState.Factory pendingStartStateFactory)
        {
            states = new Dictionary<Type, IExitState>()
            {
                [typeof(BootstrapState)] = boostrapStateFactory.Create(),
                [typeof(PendingStartState)] = pendingStartStateFactory.Create()
            };
        }

        public void Enter<TState>() where TState : class, IState =>
            ChangeState<TState>().Enter();

        public void Enter<TState, TParameter>(TParameter parameter)
            where TState : class, IStateWithParameter<TParameter> =>
            ChangeState<TState>().Enter(parameter);

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