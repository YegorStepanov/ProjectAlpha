using Code.Services;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States
{
    public sealed class EntitiesMovementState : IState<GameData>
    {
        private readonly ICamera _camera;
        private readonly ICameraResetter _cameraResetter;
        private readonly SpawnersItemsMover _spawnersItemsMover;

        public EntitiesMovementState(ICamera camera, ICameraResetter cameraResetter, SpawnersItemsMover spawnersItemsMover)
        {
            _camera = camera;
            _cameraResetter = cameraResetter;
            _spawnersItemsMover = spawnersItemsMover;
        }

        public UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            Vector2 oldPosition = _camera.Borders.Center;
            _cameraResetter.ResetPositionX();
            Vector2 newPosition = _camera.Borders.Center;

            Vector2 delta = newPosition - oldPosition;
            _spawnersItemsMover.ShiftPosition(delta);

            stateMachine.Enter<StickControlState, GameData>(data);
            return new UniTaskVoid();
        }
    }
}