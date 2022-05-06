using System.Threading;
using Code.Scopes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class InputManager
{
    private readonly CancellationToken _token;

    public InputManager(RootCancellationToken token) =>
        _token = token.Token;

    public async UniTask NextMouseClick() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: _token);

    public async UniTask NextMouseRelease() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: _token);
}