using System;
using Cysharp.Threading.Tasks;

namespace Code.Common
{
    public static class UniTaskHelper
    {
        public static Action Action(Func<UniTask> asyncAction) =>
            () => asyncAction().Forget();

        public static Action<T> Action<T>(Func<T, UniTaskVoid> asyncAction) =>
            t => asyncAction(t).Forget();

        public static Action<T> Action<T>(Func<T, UniTask> asyncAction) =>
            t => asyncAction(t).Forget();
    }
}
