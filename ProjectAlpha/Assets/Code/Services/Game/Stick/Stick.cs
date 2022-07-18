using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Stick : SpriteEntity, IStick
{
    [SerializeField] private Transform _arrow;

    private Settings _settings;
    private IStickAnimations _animations;

    [Inject, UsedImplicitly]
    public void Construct(IStickAnimations animations, Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public void ResetStick()
    {
        transform.rotation = Quaternion.identity;
        transform.localScale = transform.localScale.WithY(0f);
    }

    public bool IsStickArrowOn(IEntity entity) =>
        entity.Borders.Inside(_arrow.position);

    public UniTask StartIncreasingAsync(CancellationToken stopToken) =>
        _animations.StartIncreasing(transform, _settings.IncreaseSpeed, stopToken);

    public UniTask RotateAsync() =>
        _animations.Rotate(transform, _settings.EndRotation, _settings.RotationTime, _settings.RotationDelay, token);

    [System.Serializable]
    public class Settings
    {
        public float IncreaseSpeed = 25f;
        public Vector3 EndRotation = new(0, 0, -90);
        public float RotationTime = 0.4f;
        public float RotationDelay = 0.3f;
    }
}
