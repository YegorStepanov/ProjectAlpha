using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IInputManager
{
    UniTask WaitClick();
    UniTask WaitClick(CancellationToken token);
    UniTask WaitRelease();
    IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable();
}