using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

namespace Code.Project
{
    public sealed class StartGameButtonTrigger
    {
        private AsyncPointerClickTrigger trigger;
        
        public bool IsTriggerExist => trigger != null;

        public void SetTrigger(AsyncPointerClickTrigger trigger) =>
            this.trigger = trigger;

        public UniTask OnClickAsync() => trigger != null 
            ? trigger.OnPointerClickAsync() 
            : UniTask.CompletedTask;
    }
}