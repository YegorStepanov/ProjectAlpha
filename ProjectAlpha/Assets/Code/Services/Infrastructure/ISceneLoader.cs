using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services.Infrastructure
{
    public interface ISceneLoader
    {
        bool IsLoaded<TScene>() where TScene : struct, IScene;
        UniTask LoadAsync<TScene>() where TScene : struct, IScene;
        UniTask LoadAsync<TScene>(LifetimeScope parentScope) where TScene : struct, IScene;
        UniTask UnloadAsync<TScene>() where TScene : struct, IScene;
    }
}