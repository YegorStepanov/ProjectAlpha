using Code.Game1;
using Zenject;

namespace Code.Game.States
{
    public sealed class BootstrapState : IState
    {
        private readonly PlatformCreator platformCreator;
        private readonly PlatformWidthGenerator platformWidthGenerator;

        public BootstrapState(PlatformCreator platformCreator, PlatformWidthGenerator platformWidthGenerator)
        {
            this.platformCreator = platformCreator;
            this.platformWidthGenerator = platformWidthGenerator;
        }

        public void Enter()
        {
            platformWidthGenerator.Reset();

            platformCreator.CreateMenuPlatform();
            platformCreator.CreatePlatform();
        }
        
        public void Exit() { }

        public class Factory : PlaceholderFactory<BootstrapState> { }
    }
}