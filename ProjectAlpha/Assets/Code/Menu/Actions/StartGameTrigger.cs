using System;
using System.Diagnostics.CodeAnalysis;
using Code.Project;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Zenject;

namespace Code.Menu
{
    //start game trigger
    public sealed class StartGameTrigger : MonoBehaviour, IDisposable
    {
        private GameTriggers gameTriggers;

        [Inject, SuppressMessage("ReSharper", "ParameterHidesMember")]
        public void Construct(GameTriggers gameTriggers)
        {
            this.gameTriggers = gameTriggers;
            //check if we can закрыть trigger
            gameTriggers.StartGameButtonTrigger.SetTrigger(this.GetAsyncPointerClickTrigger());
        }

        public void Dispose() =>
            gameTriggers.StartGameButtonTrigger.SetTrigger(null);
    }
}