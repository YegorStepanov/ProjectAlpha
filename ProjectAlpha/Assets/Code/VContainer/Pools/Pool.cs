namespace Code.VContainer;

public abstract class Pool<TValue> : IPool<TValue>
{
    private readonly int _initialSize;

    private TValue[] _pool;
    private int _activeCount;

    public int Count { get; }

    //rename remaining
    protected Pool(InitialSize initialSize, Capacity capacity)
    {
        _initialSize = initialSize.Value;
        _activeCount = 0;
        Count = capacity.Value;
    }
    
    public bool TrySpawn(out TValue value)
    {
        if (_pool == null)
            CreatePool();

        if (!IsIndexCorrect())
        {
            value = default;
            return false;
        }

        // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
        if (_pool[_activeCount] == null)
            _pool[_activeCount] = Create();
        
        value = _pool[_activeCount];
        
        
        _activeCount++;

        OnSpawned(value);

        return true;
    }

    public bool TryDespawn(TValue instance)
    {
        if (_pool == null)
            CreatePool();

        _activeCount--;

        if (!IsIndexCorrect())
        {
            _activeCount++;
            return false;
        }

        _pool[_activeCount] = instance;

        OnDespawned(instance);
        
        return true;
    }

    protected abstract TValue Create();

    protected abstract void OnSpawned(TValue instance);

    protected abstract void OnDespawned(TValue instance);

    private void CreatePool()
    {
        _pool = new TValue[Count];

        for (int i = 0; i < _initialSize; i++)
        {
            _pool[i] = Create();
            OnDespawned(_pool[i]);
        }

        OnSpawned(_pool[0]);
    }

    private bool IsIndexCorrect() =>
        _activeCount >= 0 && _activeCount < _pool.Length;
}