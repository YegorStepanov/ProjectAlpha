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

        public UniTask OnClickAsync()
        {
            return trigger.OnPointerClickAsync();
        }
    }
}