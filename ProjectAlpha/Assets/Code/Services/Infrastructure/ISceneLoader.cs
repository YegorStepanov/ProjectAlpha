using System.Threading;
using Code.AddressableAssets;
using Code.Common;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services.Infrastructure;

public interface ISceneLoader
{
    bool IsLoaded(Address<Scene> scene);
    UniTask LoadAsync(Address<Scene> scene, CancellationToken token);
    UniTask LoadAsync(Address<Scene> scene, LifetimeScope parentScope, CancellationToken token);
    UniTask UnloadAsync(Address<Scene> scene, CancellationToken token);
}
