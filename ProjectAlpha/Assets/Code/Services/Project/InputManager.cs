using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
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

    public UniTask NextMouseClick(CancellationToken token) =>
        UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token).SuppressCancellationThrow();

    public async UniTask NextMouseRelease() =>
        await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: _token);
    
    public IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable() =>
        UniTaskAsyncEnumerable.Create<AsyncUnit>(async (writer, token) =>
        {
            await NextMouseClick(token);
            while (!token.IsCancellationRequested)
            {
                await writer.YieldAsync(AsyncUnit.Default);
                await NextMouseClick(token);
            }
        });
}