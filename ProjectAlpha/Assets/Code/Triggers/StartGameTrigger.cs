using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

namespace Code.Triggers;

public sealed class StartGameTrigger
{
    private AsyncPointerClickTrigger _trigger;

    public bool IsTriggerExist => _trigger != null;

    public void SetTrigger(AsyncPointerClickTrigger trigger) =>
        _trigger = trigger;

    public async UniTask OnClickAsync()
    {
        await UniTask.WaitWhile(() => _trigger == null);
        await _trigger.OnPointerClickAsync();
    }
}