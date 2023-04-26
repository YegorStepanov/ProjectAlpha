using System;
using System.Reflection;
using System.Threading;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes
{
    public abstract class Scope : LifetimeScope
    {
        private IAddressablesLoader _loader;
        private CancellationToken _token;

        public bool IsBuilt { get; private set; }

        public async UniTask WaitForBuild()
        {
            while (!IsBuilt)
                await UniTask.Yield(_token);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_token);
            ConfigureServices(builder);
        }

        protected abstract void ConfigureServices(IContainerBuilder builder);
        protected abstract UniTask PreloadAsync(IAddressablesLoader loader);

        protected sealed override void OnDestroy()
        {
            _loader?.Dispose();
            base.OnDestroy();
        }

        [UsedImplicitly]
        private new async UniTask Awake()
        {
            AssertAutoRunIsDisabled();

            _token = this.GetCancellationTokenOnDestroy();

            await InitializeRoot();
            SetParentScope();
            await BuildAsync();
        }

        private static async UniTask InitializeRoot()
        {
            if (VContainerSettings.Instance.RootLifetimeScope == null)
                return;

            if (VContainerSettings.Instance.RootLifetimeScope is not Scope rootScope)
            {
                Debug.LogWarning("RootScope should be Scope type instead LifetimeScope");
                return;
            }

            if (!IsScopeCreated(rootScope))
                await rootScope.BuildAsync();

            static bool IsScopeCreated(Scope scope) =>
                scope.Container != null;
        }

        private void SetParentScope() =>
            base.Awake();

        private async UniTask BuildAsync()
        {
            //We cannot set active scene here
            var creator = new CreatorWithLazyResolver(this);
            _loader = new AddressablesLoader(creator);
            await PreloadAsync(_loader);

            //A root scene is not real scene
            if (!IsRoot)
            {
                Scene scene = gameObject.scene;

                while (!scene.isLoaded)
                    await UniTask.Yield(_token);

                //VContainer spawns new gameobjects in the active scene
                SceneManager.SetActiveScene(scene);
            }

            Build();
            creator.Resolver = Container;
            IsBuilt = true;
        }

        private void AssertAutoRunIsDisabled()
        {
            if (!PlatformInfo.IsDevelopment) return;

            bool autoRun = GetPrivateInstanceField(typeof(LifetimeScope), this, "autoRun");
            Debug.Assert(!autoRun);

            static bool GetPrivateInstanceField(Type type, object instance, string fieldName)
            {
                FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                return (bool)field!.GetValue(instance);
            }
        }
    }
}
