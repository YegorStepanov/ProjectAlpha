using System;
using System.Diagnostics.CodeAnalysis;
using Code.Services;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Zenject;

namespace Code.UI
{
    public sealed class StartGameTrigger : MonoBehaviour, IDisposable
    {
        private GameTriggers gameTriggers;

        [Inject]
        public void Construct(GameTriggers gameTriggers)
        {
            this.gameTriggers = gameTriggers;
            gameTriggers.StartGameTrigger.SetTrigger(this.GetAsyncPointerClickTrigger());
        }

        public void Dispose() =>
            gameTriggers.StartGameTrigger.SetTrigger(null);
    }
}