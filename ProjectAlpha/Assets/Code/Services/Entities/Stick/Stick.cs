using System.Threading;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Code.Services.Entities;

public sealed class Stick : SpriteEntity, IStick
{
    [SerializeField] private Transform _arrow;

    private Settings _settings;
    private IStickAnimations _animations;

    [Inject, UsedImplicitly]
    private void Construct(IStickAnimations animations, Settings settings)
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

    public void Increasing(CancellationToken token) =>
        _animations.Increasing(transform, _settings.IncreaseSpeed, token);

    public UniTask RotateAsync() =>
        _animations.Rotate(transform, _settings.EndRotation, _settings.RotationTime, _settings.RotationDelay, DestroyToken);

    public UniTask RotateDownAsync() =>
        _animations.Rotate(transform, _settings.EndDownRotation, _settings.RotationTime, 0f, DestroyToken);

    [System.Serializable]
    public class Settings
    {
        public float IncreaseSpeed = 25f;
        public float RotationTime = 0.4f;
        public float RotationDelay = 0.3f;
        [ReadOnly]
        public Vector3 EndRotation = new(0, 0, -90);
        [ReadOnly]
        public Vector3 EndDownRotation = new(0, 0, -180);
    }
}
