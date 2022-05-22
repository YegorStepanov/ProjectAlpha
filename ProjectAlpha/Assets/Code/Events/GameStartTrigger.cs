using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

namespace Code.Triggers;

public sealed class GameStartTrigger
{
    private AsyncPointerClickTrigger _trigger;

    public void SetTrigger(AsyncPointerClickTrigger trigger) =>
        _trigger = trigger;

    public async UniTask Await()
    {
        if (StartupSceneInfo.IsGameScene) return;

        if (_trigger == null)
            await UniTask.WaitWhile(() => _trigger == null);

        await _trigger.OnPointerClickAsync();
    }
}