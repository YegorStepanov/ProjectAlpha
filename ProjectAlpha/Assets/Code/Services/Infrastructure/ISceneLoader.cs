using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services.Infrastructure;

public interface ISceneLoader
{
    bool IsLoaded<TScene>()
        where TScene : struct, IScene;

    UniTask LoadAsync<TScene>(CancellationToken token)
        where TScene : struct, IScene;

    UniTask LoadAsync<TScene>(LifetimeScope parentScope, CancellationToken token)
        where TScene : struct, IScene;

    UniTask UnloadAsync<TScene>(CancellationToken token)
        where TScene : struct, IScene;
}
