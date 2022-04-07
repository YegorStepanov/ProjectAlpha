using System;
using System.Collections;
using Code.Annotations;
using FluentAssertions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace Code.Common
{
    [CreateAssetMenu(menuName = "SO/Scenes")]
    [ZenjectAllowDuringValidation]
    public sealed class SceneReferences : ScriptableObject
    {
        public SceneReference BootstrapScene;
        public SceneReference MenuScene;
        public SceneReference GameScene;
        public SceneReference MiniGameScene;

        private void Awake() =>
            Debug.Log("SO.Awake2");

        private void Reset() =>
            Debug.Log("SO.Reset");

        private void OnDisable() =>
            Debug.Log("SO.OnDisable");

        private void OnDestroy() =>
            Debug.Log("SO.OnDestroy");

        private void OnEnable() =>
            Debug.Log("SO.OnEnable");
    }
}