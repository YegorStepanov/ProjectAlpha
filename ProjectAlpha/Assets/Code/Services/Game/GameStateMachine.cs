﻿using System;
using System.Collections.Generic;
using Code.States;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services;

public sealed class GameStateMachine : IStateMachine
{
    private readonly Dictionary<Type, IExitState> _states;

    private IExitState _activeState;

    [Inject, UsedImplicitly]
    public GameStateMachine(IObjectResolver resolver) => _states = new Dictionary<Type, IExitState>
    {
        [typeof(BootstrapState)] = resolver.ResolveInstance<BootstrapState>(),
        [typeof(HeroMovementState)] = resolver.ResolveInstance<HeroMovementState>(),
        [typeof(GameStartState)] = resolver.ResolveInstance<GameStartState>(),
        [typeof(StickControlState)] = resolver.ResolveInstance<StickControlState>(),
        [typeof(MoveHeroToNextPlatformState)] = resolver.ResolveInstance<MoveHeroToNextPlatformState>(),
        [typeof(RestartState)] = resolver.ResolveInstance<RestartState>(),
    };

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