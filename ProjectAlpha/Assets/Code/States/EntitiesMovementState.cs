using Code.Services;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class EntitiesMovementState : IState<GameData>
{
    private readonly ICamera _camera;
    private readonly ICameraRestorer _cameraRestorer;
    private readonly SpawnersItemsMover _spawnersItemsMover;

    public EntitiesMovementState(ICamera camera, ICameraRestorer cameraRestorer, SpawnersItemsMover spawnersItemsMover)
    {
        _camera = camera;
        _cameraRestorer = cameraRestorer;
        _spawnersItemsMover = spawnersItemsMover;
    }

    public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
    {
        //todo: if !settings return

        await UniTask.Delay(1000);

        Vector2 oldPosition = _camera.Borders.Center;
        _cameraRestorer.RestorePositionX();
        Vector2 newPosition = _camera.Borders.Center;

        Vector2 delta = newPosition - oldPosition;
        _spawnersItemsMover.ShiftPosition(delta, data.Hero);

        stateMachine.Enter<StickControlState, GameData>(data);
    }
}
