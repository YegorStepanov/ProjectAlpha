using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
internal interface IAddressableAssetCache<out TAsset> : IDisposable where TAsset : Object
{
    UniTask<int> CacheAssetAsync(string address);
    int ReleaseCachedAsset(string address);
    void RemoveCachedAsset(string address);
}