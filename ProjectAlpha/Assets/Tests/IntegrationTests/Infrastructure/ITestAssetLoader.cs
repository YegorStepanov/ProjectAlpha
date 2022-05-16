using System;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Tests;

public interface ITestAssetLoader<T>: IDisposable where T : Object
{
    UniTask<T> LoadAsset();
    UniTask<AsyncOperationHandle<T>> LoadAssetHandle();
}