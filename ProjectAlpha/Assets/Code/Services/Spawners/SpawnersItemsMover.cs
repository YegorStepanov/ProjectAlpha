using Code.Services.Entities;
using UnityEngine;

namespace Code.Services.Spawners
{
    public sealed class SpawnersItemsMover
    {
        private readonly HeroSpawner _heroSpawner;
        private readonly PlatformSpawner _platformSpawner;
        private readonly CherrySpawner _cherrySpawner;
        private readonly StickSpawner _stickSpawner;

        public SpawnersItemsMover(HeroSpawner heroSpawner, PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, StickSpawner stickSpawner)
        {
            _heroSpawner = heroSpawner;
            _platformSpawner = platformSpawner;
            _cherrySpawner = cherrySpawner;
            _stickSpawner = stickSpawner;
        }

        public void ShiftPosition(Vector2 distance)
        {
            ShiftActiveItems<HeroSpawner, Hero>(_heroSpawner, distance);
            ShiftActiveItems<PlatformSpawner, Platform>(_platformSpawner, distance);
            ShiftActiveItems<CherrySpawner, Cherry>(_cherrySpawner, distance);
            ShiftActiveItems<StickSpawner, Stick>(_stickSpawner, distance);
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
}