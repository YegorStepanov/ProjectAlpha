using Cysharp.Threading.Tasks;

namespace Code.Services.Infrastructure;

public interface IInputManager
{
    UniTask WaitClick();
    UniTask WaitClickRelease();
    IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable();
}
