using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IInputManager
{
    UniTask WaitClick();
    UniTask WaitClickRelease();
    IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable();
}
