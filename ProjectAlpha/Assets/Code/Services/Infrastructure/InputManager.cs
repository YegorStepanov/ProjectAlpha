﻿using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.InputSystem;

namespace Code.Services.Infrastructure;

public sealed class InputManager : IInputManager
{
    private readonly CancellationToken _token;
    private readonly InputAction _action = new(binding: "*/{primaryAction}");

    public InputManager(CancellationToken token)
    {
        _token = token;
        _action.Enable();
    }

    public UniTask WaitClick() =>
        WaitClick(_token);

    public async UniTask WaitClickRelease()
    {
        await UniTask.NextFrame(_token);
        while (!IsReleasedOrCancelled())
            await UniTask.NextFrame(_token);
    }

    public IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable() =>
        UniTaskAsyncEnumerable.Create<AsyncUnit>(async (writer, token) =>
        {
            await WaitClick(token);
            while (!token.IsCancellationRequested)
            {
                await writer.YieldAsync(AsyncUnit.Default);
                await WaitClick(token);
            }
        });

    private async UniTask WaitClick(CancellationToken token)
    {
        await UniTask.NextFrame(token);
        while (!IsPressedOrCancelled(token))
            await UniTask.NextFrame(token);
    }

    private bool IsPressedOrCancelled(CancellationToken token) =>
        IsPressedOrCancelled() || token.IsCancellationRequested;

    private bool IsPressedOrCancelled() =>
        _action.WasPressedThisFrame() || _token.IsCancellationRequested;

    private bool IsReleasedOrCancelled() =>
        _action.WasReleasedThisFrame() || _token.IsCancellationRequested;
}