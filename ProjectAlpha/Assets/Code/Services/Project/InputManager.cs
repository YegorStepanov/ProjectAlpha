using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class InputManager
{
    private readonly CancellationToken token;

    public InputManager(CancellationToken token) =>
        this.token = token;

    public async UniTask NextMouseClick() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);

    public async UniTask NextMouseRelease() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: token);
}