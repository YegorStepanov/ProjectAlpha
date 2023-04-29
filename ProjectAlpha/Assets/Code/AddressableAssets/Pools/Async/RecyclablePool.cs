using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Code.AddressableAssets
{
//it will be the decorator when VContainer adds support for it
    public sealed class RecyclablePool<T> : IAsyncPool<T>
    {
        private readonly IAsyncPool<T> _pool;
        private readonly List<T> _cachedItems;
        private int _cacheIndex;

        public int Capacity { get; }
        public bool CanBeSpawned => _pool.CanBeSpawned;

        public RecyclablePool(IAsyncPool<T> pool)
        {
            _pool = pool;
            _cachedItems = new List<T>(_pool.Capacity);
            Capacity = _pool.Capacity;
        }

        public async UniTask<T> SpawnAsync() => CanBeSpawned
            ? await SpawnFromPool()
            : GetFromCache();

        public void Despawn(T value)
        {
            TryRemoveFromCache(value);
            _pool.Despawn(value);
        }

        public void DespawnAll()
        {
            foreach (T item in _cachedItems)
                _pool.Despawn(item);

            _cacheIndex = 0;
            _cachedItems.Clear();
        }

        private async UniTask<T> SpawnFromPool()
        {
            T item = await _pool.SpawnAsync();
            _cachedItems.Add(item);
            return item;
        }

        private T GetFromCache()
        {
            T item = _cachedItems[_cacheIndex];
            _cacheIndex = (_cacheIndex + 1) % _cachedItems.Count;
            return item;
        }

        private void TryRemoveFromCache(T value)
        {
            int index = _cachedItems.IndexOf(value);
            if (index == -1) return;
            _cachedItems.RemoveAt(index);

            if (_cacheIndex > 0 && index > _cacheIndex)
                _cacheIndex--;
        }
    }
}