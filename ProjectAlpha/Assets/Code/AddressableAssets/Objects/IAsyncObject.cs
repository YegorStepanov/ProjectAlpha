using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public interface IAsyncObject<T> where T : Object
{
    bool IsLoaded { get; }
    UniTask<T> GetAssetAsync();
    T GetAsset();
}