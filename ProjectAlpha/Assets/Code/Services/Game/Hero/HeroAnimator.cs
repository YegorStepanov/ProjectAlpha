using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.HeroAnimators;

public sealed class HeroAnimator : MonoBehaviour, IAnimationStateReader
{
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Stay = Animator.StringToHash("Stay");

    private static readonly int KickState = Animator.StringToHash("kick");
    private static readonly int MoveState = Animator.StringToHash("walk");
    private static readonly int StayState = Animator.StringToHash("stay");

    public HeroAnimatorState State { get; private set; }

    [SerializeField] private Animator _animator;

    private bool _hitEvent;

    [UsedImplicitly]
    private void HitEvent() =>
        _hitEvent = true;

    public async UniTask PlayKickAsync()
    {
        _animator.SetTrigger(Kick);

        await UniTask.WaitWhile(() => _hitEvent == false);
        _hitEvent = false;
    }

    public void PlayMove() =>
        _animator.SetTrigger(Move);

    public void PlayStay() =>
        _animator.SetTrigger(Stay);

    public void EnteredState(int stateHash) =>
        State = GetState(stateHash);

    public void ExitedState(int stateHash) =>
        State = GetState(stateHash);

    private static HeroAnimatorState GetState(int stateHash)
    {
        if (stateHash == KickState) return HeroAnimatorState.Kick;
        if (stateHash == MoveState) return HeroAnimatorState.Move;
        if (stateHash == StayState) return HeroAnimatorState.Stay;

        throw new ArgumentException($"Unknown state hash: {stateHash}");
    }
}