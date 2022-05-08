﻿using Code.Services;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.UI;

public sealed class StartGameTrigger : MonoBehaviour
{
    private GameTriggers _gameTriggers;

    [Inject, UsedImplicitly]
    public void Construct(GameTriggers gameTriggers)
    {
        _gameTriggers = gameTriggers;
        _gameTriggers.StartGameTrigger.SetTrigger(this.GetAsyncPointerClickTrigger());
    }

    private void OnDestroy() =>
        _gameTriggers.StartGameTrigger.SetTrigger(null);
}