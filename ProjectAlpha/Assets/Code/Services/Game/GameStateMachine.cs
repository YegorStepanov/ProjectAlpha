using System;
using System.Collections.Generic;
using System.Linq;
using Code.States;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services;

public sealed class GameStateMachine : IGameStateMachine
{
    private readonly Dictionary<Type, IExitState> _states;

    [Inject, UsedImplicitly]
    public GameStateMachine(IEnumerable<IExitState> states) =>
        _states = states.ToDictionary(s => s.GetType());

    public void Enter<TState>() where TState : class, IState =>
        GetState<TState>().EnterAsync(this).Forget();

    public void Enter<TState, TArg>(TArg argument) where TState : class, IState<TArg> =>
        GetState<TState>().EnterAsync(argument, this).Forget();

    private TState GetState<TState>() where TState : class, IExitState =>
        _states[typeof(TState)] as TState;

    [System.Serializable]
    public class Settings
    {
        public float DelayBeforeNextState = 0.1f;
        public float DelayBeforeEndGame = 0.3f;
        public float DelayBeforeHeroMovement = 0.2f;
    }
}
