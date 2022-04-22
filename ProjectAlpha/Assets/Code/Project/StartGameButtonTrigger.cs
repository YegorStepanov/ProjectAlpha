using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;

namespace Code.Project
{
    public sealed class StartGameButtonTrigger : AwaitableEvent
    {
        private AsyncPointerClickTrigger trigger;
        public bool ExistTrigger => trigger != null;

        public void SetTrigger(AsyncPointerClickTrigger _trigger) =>
            trigger = _trigger;

        public UniTask OnClickAsync() => trigger.OnPointerClickAsync();
    }
}