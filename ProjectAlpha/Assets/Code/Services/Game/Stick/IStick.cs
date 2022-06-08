using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IStick : IEntity
{
    bool IsStickArrowOn(IEntity entity);
    UniTask StartIncreasingAsync(CancellationToken token);
    UniTask RotateAsync();
}