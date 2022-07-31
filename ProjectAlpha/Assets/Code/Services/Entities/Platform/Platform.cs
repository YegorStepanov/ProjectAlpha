using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Code.Services.Entities;

public sealed class Platform : SpriteEntity, IPlatform
{
    [SerializeField] private PlatformRedPoint _platformRedPoint;
    private IPlatformAnimations _animations;
    private Settings _settings;

    public IPlatformRedPoint PlatformRedPoint => _platformRedPoint;

    [Inject]
    public void Construct(IPlatformAnimations animations, Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public UniTask MoveAsync(float destinationX) =>
        _animations.Move(transform, destinationX, _settings.MovementSpeed, DestroyToken);

    [System.Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
        public float FadeOutRedPointSpeed = 0.3f;
    }
}
