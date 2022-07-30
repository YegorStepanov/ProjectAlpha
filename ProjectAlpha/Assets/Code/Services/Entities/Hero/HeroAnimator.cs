using System;
using System.Threading;
using Code.Animations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Entities.Hero;

public sealed class HeroAnimator : MonoBehaviour, IAnimationStateReader
{
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Stay = Animator.StringToHash("Stay");

    private static readonly int KickState = Animator.StringToHash("kick");
    private static readonly int MoveState = Animator.StringToHash("walk");
    private static readonly int StayState = Animator.StringToHash("stay");

    [SerializeField] private Animator _animator;

    private AsyncReactiveProperty<HeroAnimatorState>[] _exitStates;

    private void Awake()
    {
        _exitStates = new AsyncReactiveProperty<HeroAnimatorState>[2];

        for (int i = 0; i < _exitStates.Length; i++)
            _exitStates[i] = new AsyncReactiveProperty<HeroAnimatorState>(HeroAnimatorState.None);
    }

    public UniTask PlayKickAsync(CancellationToken token) =>
        Play(Kick, HeroAnimatorState.Kick, 0, token);

    public void PlayMove() =>
        _animator.SetTrigger(Move);

    public void PlayStay() =>
        _animator.SetTrigger(Stay);

    public void EnteredState(int stateHash, int layerIndex) { }

    public void ExitedState(int stateHash, int layerIndex) =>
        GetState(layerIndex).Value = GetStateFromHash(stateHash);

    private async UniTask Play(
        int stateHash, HeroAnimatorState heroAnimatorState, int layerIndex, CancellationToken token)
    {
        _animator.SetTrigger(stateHash);

        AsyncReactiveProperty<HeroAnimatorState> state = GetState(layerIndex);

        //sometimes the new state can be set in this frame
        Debug.Log("Before " + Time.frameCount + "  " + state.Value);

        var newState = HeroAnimatorState.None;
        while (!token.IsCancellationRequested)
        {
            if (newState == heroAnimatorState) break;

            newState = await state.WaitAsync(token);
        }

        Debug.Log("After " + Time.frameCount + "  " + state.Value);
    }

    private static HeroAnimatorState GetStateFromHash(int stateHash)
    {
        if (stateHash == KickState) return HeroAnimatorState.Kick;
        if (stateHash == MoveState) return HeroAnimatorState.Move;
        if (stateHash == StayState) return HeroAnimatorState.Stay;

        throw new ArgumentException($"Unknown state hash: {stateHash}");
    }

    private AsyncReactiveProperty<HeroAnimatorState> GetState(int layerIndex) =>
        _exitStates[layerIndex];
}
