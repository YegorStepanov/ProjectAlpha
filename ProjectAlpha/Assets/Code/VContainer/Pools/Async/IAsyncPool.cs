using Cysharp.Threading.Tasks;

namespace Code.VContainer;

public interface IAsyncPool<T>
{
    int Capacity { get; }
    UniTask<(T, bool)> SpawnAsync();
    void Despawn(T value);
}