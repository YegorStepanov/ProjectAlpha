using Cysharp.Threading.Tasks;

namespace Code.Common
{
    public readonly struct UniTaskDisposable
    {
        public readonly UniTask UniTask;

        private UniTaskDisposable(UniTask uniTask) =>
            UniTask = uniTask;

        public UniTask DisposeAsync() =>
            UniTask;

        public static implicit operator UniTaskDisposable(UniTask uniTask) =>
            new(uniTask);
    }
}
