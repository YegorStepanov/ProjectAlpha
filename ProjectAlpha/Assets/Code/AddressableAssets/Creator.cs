using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.AddressableAssets
{
    public sealed class CreatorWithLazyResolver : IObjectCreator
    {
        private readonly Creator _creator;

        public IObjectResolver Resolver { get; set; }

        public CreatorWithLazyResolver(LifetimeScope scope)
        {
            _creator = new Creator(scope);
        }

        public GameObject InstantiateEmpty(string name)
        {
            return _creator.InstantiateEmpty(name);
        }

        public T Instantiate<T>(T prefab) where T : Object
        {
            T instance = _creator.InstantiateNoInject(prefab);
            InjectUnityEngineObject(instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance = _creator.InstantiateNoInject(prefab, position, rotation);
            InjectUnityEngineObject(instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = _creator.InstantiateNoInject(prefab, position, rotation, parent);
            InjectUnityEngineObject(instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
        {
            T instance = _creator.InstantiateNoInject(prefab, parent, worldPositionStays);
            InjectUnityEngineObject(instance);
            return instance;
        }

        public T InstantiateNoInject<T>(T prefab) where T : Object
        {
            return _creator.InstantiateNoInject(prefab);
        }

        public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return _creator.InstantiateNoInject(prefab, position, rotation);
        }

        public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            return _creator.InstantiateNoInject(prefab, position, rotation, parent);
        }

        public T InstantiateNoInject<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
        {
            return _creator.InstantiateNoInject(prefab, parent, worldPositionStays);
        }

        private void InjectUnityEngineObject<T>(T instance) where T : UnityEngine.Object
        {
            IObjectResolver resolver = Resolver;

            Debug.Assert(resolver != null);

            if (instance is GameObject gameObject)
                resolver.InjectGameObject(gameObject);
            else
                resolver.Inject(instance);
        }
    }

    public sealed class Creator : IObjectCreator
    {
        private readonly LifetimeScope _scope;
        private readonly Transform _scopeTransform;
        // We cannot set the value immediately in the constructor because the current scope is not fully created, so the Container property is null
        private IObjectResolver Resolver => _scope.Container;

        public Creator(LifetimeScope scope)
        {
            _scope = scope;
            _scopeTransform = scope.transform;
        }

        public GameObject InstantiateEmpty(string name)
        {
            var instance = new GameObject(name);

            Transform t = instance.transform;
            t.parent = _scopeTransform;
            // ReSharper disable once Unity.InefficientPropertyAccess
            t.parent = null;

            return instance;
        }

        public T Instantiate<T>(T prefab) where T : Object
        {
            T instance = InstantiateNoInject(prefab);
            InjectUnityEngineObject(Resolver, instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance = InstantiateNoInject(prefab, position, rotation);
            InjectUnityEngineObject(Resolver, instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = InstantiateNoInject(prefab, position, rotation, parent);
            InjectUnityEngineObject(Resolver, instance);
            return instance;
        }

        public T Instantiate<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
        {
            T instance = InstantiateNoInject(prefab, parent, worldPositionStays);
            InjectUnityEngineObject(Resolver, instance);
            return instance;
        }

        public T InstantiateNoInject<T>(T prefab) where T : Object
        {
            T instance;
            if (_scope.IsRoot)
            {
                instance = UnityEngine.Object.Instantiate(prefab);
                UnityEngine.Object.DontDestroyOnLoad(instance);
            }
            else
            {
                instance = UnityEngine.Object.Instantiate(prefab, _scopeTransform);
                ResetParent(instance);
            }

            return instance;
        }

        public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance;
            if (_scope.IsRoot)
            {
                instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
                UnityEngine.Object.DontDestroyOnLoad(instance);
            }
            else
            {
                instance = UnityEngine.Object.Instantiate(prefab, position, rotation, _scopeTransform);
                ResetParent(instance);
            }

            return instance;
        }

        public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            return UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
        }

        public T InstantiateNoInject<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
        {
            return UnityEngine.Object.Instantiate(prefab, parent, worldPositionStays);
        }

        private static void InjectUnityEngineObject<T>(IObjectResolver resolver, T instance) where T : UnityEngine.Object
        {
            if (instance is GameObject gameObject)
                resolver.InjectGameObject(gameObject);
            else
                resolver.Inject(instance);
        }

        //TODO worldPositionStays
        private static void ResetParent<T>(T instance) where T : UnityEngine.Object
        {
            switch (instance)
            {
                case Component component:
                    component.transform.SetParent(null);
                    break;
                case GameObject gameObject:
                    gameObject.transform.SetParent(null);
                    break;
            }
        }
    }
}
