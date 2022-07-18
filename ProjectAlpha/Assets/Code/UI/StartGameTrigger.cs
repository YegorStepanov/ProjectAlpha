using Code.Services;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.UI;

public sealed class StartGameTrigger : MonoBehaviour
{
    [Inject, UsedImplicitly]
    public void Construct(GameEvents gameEvents)
    {
        var clickStream = this.GetAsyncPointerClickTrigger().Select(_ => AsyncUnit.Default);

        gameEvents.GameStarted.SetTrigger(clickStream, this.GetCancellationTokenOnDestroy());
    }
}
