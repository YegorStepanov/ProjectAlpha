using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets
{
    public interface IAddressablesCache
    {
        UniTask CacheAssetAsync<T>(Address<T> address) where T : Object;
        void RemoveCachedAsset<T>(Address<T> address) where T : Object;
        void RemoveAllCachedAssets<T>(Address<T> address) where T : Object;
    }
}