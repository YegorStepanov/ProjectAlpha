using System;
using Code.Services;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Zenject;

namespace Code.UI;

public sealed class StartGameTrigger : MonoBehaviour //, IDisposable
{
    private GameTriggers _gameTriggers;

    public void Dispose() =>
        _gameTriggers.StartGameTrigger.SetTrigger(null);

    [Inject]
    public void Construct(GameTriggers gameTriggers)
    {
        _gameTriggers = gameTriggers;
        gameTriggers.StartGameTrigger.SetTrigger(this.GetAsyncPointerClickTrigger());
    }
}