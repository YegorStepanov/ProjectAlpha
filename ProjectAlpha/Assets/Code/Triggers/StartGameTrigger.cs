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
        if (StartupSceneInfo.IsStartedInGameStartScene)
            return;
        
        if(_trigger == null)
            await UniTask.WaitWhile(() => _trigger == null);
      
        await _trigger.OnPointerClickAsync();
    }
}