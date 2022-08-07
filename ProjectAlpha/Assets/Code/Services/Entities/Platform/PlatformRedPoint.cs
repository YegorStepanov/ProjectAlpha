using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services.Entities;

public sealed class PlatformRedPoint : SlicedSpriteEntity, IPlatformRedPoint
{
    private IPlatformAnimations _animations;
    private Platform.Settings _settings;

    [Inject, UsedImplicitly]
    private void Construct(IPlatformAnimations animations, Platform.Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public void Toggle(bool enable) =>
        _sprite.color = _sprite.color with { a = enable ? 1 : 0 };

    public UniTask FadeOutAsync() =>
        _animations.FadeOut(_sprite, _settings.FadeOutRedPointSpeed, DestroyToken);
}
