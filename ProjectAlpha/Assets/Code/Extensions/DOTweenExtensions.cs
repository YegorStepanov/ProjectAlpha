using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class DOTweenExtensions
{
    public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveUV(
        this RawImage target, Vector2 endOffset, float duration)
    {
        TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(
            () => target.uvRect.position,
            position => target.uvRect = target.uvRect with { position = position },
            endOffset,
            duration);

        t.SetTarget(target);
        return t;
    }

    public static TweenerCore<float, float, FloatOptions> DOMoveUVX(
        this RawImage target, float endValue, float duration)
    {
        TweenerCore<float, float, FloatOptions> t = DOTween.To(
            () => target.uvRect.x,
            x => target.uvRect = target.uvRect with { x = x },
            endValue,
            duration);

        t.SetTarget(target);
        return t;
    }

    public static TweenerCore<float, float, FloatOptions> DOMoveUVY(
        this RawImage target, float endValue, float duration)
    {
        TweenerCore<float, float, FloatOptions> t = DOTween.To(
            () => target.uvRect.y,
            y => target.uvRect = target.uvRect with { y = y },
            endValue,
            duration);

        t.SetTarget(target);
        return t;
    }
}
