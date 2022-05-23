using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tayx.Graphy;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IAsyncStartable
{
    public RootStart(CameraController camera, GameTriggers gameTriggers, GraphyManager graphy, EventSystem eventSystem)
    {
        //aka NonLazy()
        _ = camera;
        _ = gameTriggers;
        _ = graphy;
        _ = eventSystem;
    }

    public UniTask StartAsync(CancellationToken token)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);

        return UniTask.CompletedTask;
    }
}