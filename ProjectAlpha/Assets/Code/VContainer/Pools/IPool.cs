namespace Code.VContainer;

public interface IPool<TValue>
{
    int Count { get; }
    bool TrySpawn(out TValue value);
    bool TryDespawn(TValue value);
}