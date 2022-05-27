using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public abstract class AsyncObject<T> : IDisposable where T : Object
{
    private readonly IScopedAddressablesLoader _loader;

    protected T resource;

    public bool IsLoaded => resource != null;

    protected AsyncObject(IScopedAddressablesLoader loader) =>
        _loader = loader;

    public T GetAsset() => resource;

    public async UniTask<T> GetAssetAsync()
    {
        if (resource == null)
            await WaitResource();

        return resource;
    }

    public void Dispose()
    {
        if (resource != null)
            _loader.Release(resource);
    }

    protected abstract UniTask WaitResource();
}