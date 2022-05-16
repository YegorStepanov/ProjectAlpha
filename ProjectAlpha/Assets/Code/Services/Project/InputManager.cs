using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Code.Services;

public sealed class InputManager
{
    private readonly CancellationToken _token;

    public InputManager(LifetimeScope scope) =>
        _token = scope.transform.GetCancellationTokenOnDestroy();

    public async UniTask NextMouseClick() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: _token);

    public async UniTask NextMouseRelease() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: _token);
}