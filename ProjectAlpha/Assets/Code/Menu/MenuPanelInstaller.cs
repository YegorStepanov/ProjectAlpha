using UnityEngine;
using Zenject;

namespace Code.Game
{
    public sealed class MenuPanelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject MenuPanel;

        public override void InstallBindings()
        {
            Container.Inject(MenuPanel);
            //.BindInterfacesAndSelfTo<GameState>().FromInstance(_myGameStateReference).AsSingle();
        }
    }
}