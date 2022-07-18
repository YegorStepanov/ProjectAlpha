using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.AddressableAssets;

public readonly record struct HandleData<TAsset>(AsyncOperationHandle<TAsset> Handle, Address<TAsset> Address)
    where TAsset : Object
{
    public TAsset Asset => Handle.Result;
}
