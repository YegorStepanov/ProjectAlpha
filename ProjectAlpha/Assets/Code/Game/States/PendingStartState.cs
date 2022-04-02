using Zenject;

namespace Code.Game.States
{
    public sealed class PendingStartState : IState
    {
        public PendingStartState() { }

        public void Enter() { }

        public void Exit() { }

        public class Factory : PlaceholderFactory<PendingStartState> { }
    }
}