using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IStick : IEntity
{
    bool IsStickArrowOn(IEntity entity);
    void Increasing(CancellationToken token);
    UniTask RotateAsync();
    UniTask RotateDownAsync();
}
