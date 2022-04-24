using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuMediator menu;
        private void Awake()
        {
            return;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EditorOnly"))
                Destroy(go);
        }

        public override void InstallBindings()
        {
            Debug.Log("MenuInstaller.InstallBindings" + ": " + Time.frameCount);
            Container.BindInterfacesTo<MenuInitializer>().AsSingle();
            
            Container.Bind<StartGameTrigger>().AsSingle();

            Container.Bind<UIManager>().AsSingle();

            Container.BindInstance(menu);
            
            Container.Bind<AddressableFactory>().AsSingle().WithArguments(transform);
        }
    }
}