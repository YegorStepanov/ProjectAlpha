using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Platform : SpriteEntity, IPlatform
{
    [SerializeField] private RedPoint _redPoint;
    private Settings _settings;
    private IPlatformAnimations _animations;

    public IRedPoint RedPoint => _redPoint;

    [Inject, UsedImplicitly]
    public void Construct(IPlatformAnimations animations, Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public UniTask MoveAsync(float destinationX) =>
        _animations.Move(transform, destinationX, _settings.MovementSpeed, token);

    [System.Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
        public float FadeOutRedPointSpeed = 0.3f;
    }
}