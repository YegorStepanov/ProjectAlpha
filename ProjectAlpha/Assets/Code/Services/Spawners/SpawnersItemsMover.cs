using Code.Services.Entities;
using UnityEngine;

namespace Code.Services.Spawners;

public sealed class SpawnersItemsMover
{
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly StickSpawner _stickSpawner;

    public SpawnersItemsMover(PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, StickSpawner stickSpawner)
    {
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _stickSpawner = stickSpawner;
    }

    public void ShiftPosition(Vector2 distance, IHero hero) //todo:
    {
        ShiftActiveItems<PlatformSpawner, Platform>(_platformSpawner, distance);
        ShiftActiveItems<CherrySpawner, Cherry>(_cherrySpawner, distance);
        ShiftActiveItems<StickSpawner, Stick>(_stickSpawner, distance);

        ((IPositionShifter)hero).ShiftPosition(distance);
    }

    private static void ShiftActiveItems<TSpawner, TShifter>(TSpawner spawner, Vector2 distance)
        where TSpawner : Spawner<TShifter>
        where TShifter : IPositionShifter
    {
        // Reduce allocations: ActiveItems enumerator is a class, not struct
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int index = 0; index < spawner.ActiveItems.Count; index++)
        {
            TShifter shifter = spawner.ActiveItems[index];
            shifter.ShiftPosition(distance);
        }
    }
}
