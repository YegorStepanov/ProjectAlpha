using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tayx.Graphy;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IAsyncStartable
{
    public RootStart(CameraController camera, GameTriggers gameTriggers, GraphyManager graphy)
    {
        //aka camera.NonLazy()
        _ = camera;
        _ = gameTriggers;
        _ = graphy;
    }

    public UniTask StartAsync(CancellationToken token)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);

        return UniTask.CompletedTask;
    }
}