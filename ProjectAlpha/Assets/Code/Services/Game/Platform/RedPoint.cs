using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer;

namespace Code.Services;

public sealed class RedPoint : SpriteEntity, IRedPoint
{
    private IPlatformAnimations _animations;
    private Platform.Settings _settings;

    [Inject, UsedImplicitly]
    public void Construct(IPlatformAnimations animations, Platform.Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    //todo: rename
    public bool Inside(float pointX) =>
        _sprite.color.a != 0 && pointX >= Borders.Left && pointX <= Borders.Right;

    public void Toggle(bool enable) =>
        _sprite.color = _sprite.color with { a = enable ? 1 : 0 };

    public UniTask FadeOutAsync() => 
        _animations.FadeOut(_sprite, _settings.FadeOutRedPointSpeed, token);
}