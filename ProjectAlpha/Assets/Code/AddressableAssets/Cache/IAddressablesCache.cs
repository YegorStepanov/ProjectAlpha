using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public interface IAddressablesCache
{
    UniTask<int> CacheAssetAsync<T>(Address<T> address) where T : Object;
    int ReleaseCachedAsset<T>(Address<T> address) where T : Object;
    void RemoveCachedAsset<T>(Address<T> address) where T : Object;
}