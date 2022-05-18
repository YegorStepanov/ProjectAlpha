using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
internal interface IAddressableAssetLoader<out TAsset> : IDisposable where TAsset : Object
{
    UniTask<Object> LoadAssetAsync(string address);
    void Release(Object asset);
    bool IsLoaded(Object asset);
}