using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Code.Services;

public sealed class InputManager
{
    private readonly CancellationToken _token;
    private readonly InputAction _action = new(binding: "*/{primaryAction}");

    public InputManager(LifetimeScope scope)
    {
        _token = scope.transform.GetCancellationTokenOnDestroy();
        _action.Enable();
    }

    public async UniTask NextClick()
    {
        await UniTask.NextFrame(_token);
        while (!IsPressedOrCancelled())
            await UniTask.NextFrame(_token);
    }

    public async UniTask NextClick(CancellationToken token)
    {
        await UniTask.NextFrame(token);
        while (!IsPressedOrCancelled(token))
            await UniTask.NextFrame(token);
    }

    public async UniTask NextRelease()
    {
        await UniTask.NextFrame(_token);
        while (!IsReleasedOrCancelled())
            await UniTask.NextFrame(_token);
    }

    public IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable() =>
        UniTaskAsyncEnumerable.Create<AsyncUnit>(async (writer, token) =>
        {
            await NextClick(token);
            while (!token.IsCancellationRequested)
            {
                await writer.YieldAsync(AsyncUnit.Default);
                await NextClick(token);
            }
        });

    private bool IsPressedOrCancelled(CancellationToken token) =>
        IsPressedOrCancelled() || token.IsCancellationRequested;

    private bool IsPressedOrCancelled() =>
        _action.WasPressedThisFrame() || _token.IsCancellationRequested;

    private bool IsReleasedOrCancelled() =>
        _action.WasReleasedThisFrame() || _token.IsCancellationRequested;
}