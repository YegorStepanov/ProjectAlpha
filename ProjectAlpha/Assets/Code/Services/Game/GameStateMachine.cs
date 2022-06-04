using System;
using System.Collections.Generic;
using System.Linq;
using Code.States;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services;

public sealed class GameStateMachine : IStateMachine
{
    private readonly Dictionary<Type, IExitState> _states;

    private IExitState _activeState;

    [Inject, UsedImplicitly]
    public GameStateMachine(IEnumerable<IExitState> states) =>
        _states = states.ToDictionary(s => s.GetType());

    public void Enter<TState>() where TState : class, IState =>
        ChangeState<TState>().EnterAsync(this).Forget();

    public void Enter<TState, TArg>(TArg argument) where TState : class, IState<TArg> =>
        ChangeState<TState>().EnterAsync(argument, this).Forget();

    private TState ChangeState<TState>() where TState : class, IExitState
    {
        _activeState?.Exit();

        var state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitState =>
        _states[typeof(TState)] as TState;
}