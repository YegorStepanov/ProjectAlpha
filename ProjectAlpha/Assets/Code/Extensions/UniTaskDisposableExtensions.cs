using Cysharp.Threading.Tasks;

namespace Code.Common ;

    public static class UniTaskDisposableExtensions
    {
        public static UniTaskDisposable ToUniTaskDisposable(this UniTask task) => task;
    }