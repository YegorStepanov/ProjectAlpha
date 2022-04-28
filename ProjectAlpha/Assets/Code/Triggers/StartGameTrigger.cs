using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

namespace Code.Triggers
{
    public sealed class StartGameTrigger
    {
        private AsyncPointerClickTrigger trigger;

        public bool IsTriggerExist => trigger != null;

        public void SetTrigger(AsyncPointerClickTrigger trigger) =>
            this.trigger = trigger;

        public async UniTask OnClickAsync()
        {
            await UniTask.WaitWhile(() => trigger == null);
            await trigger.OnPointerClickAsync();
        }
    }
}