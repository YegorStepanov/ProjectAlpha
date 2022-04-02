using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuInstaller : MonoInstaller
    {
        private void Awake()
        {
            return;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EditorOnly")) 
                Destroy(go);
        }

        public override void InstallBindings()
        {
            Debug.Log("MenuInstaller.InstallBindings");
            Container.BindInterfacesTo<MenuInitializer>().AsSingle();
        }
    }
}