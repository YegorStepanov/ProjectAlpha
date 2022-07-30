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
            position =>
            {
                Rect rect = target.uvRect;
                rect.position = position;
                target.uvRect = rect;
            },
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
            x =>
            {
                Rect rect = target.uvRect;
                rect.x = x;
                target.uvRect = rect;
            },
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
            y =>
            {
                Rect rect = target.uvRect;
                rect.y = y;
                target.uvRect = rect;
            },
            endValue,
            duration);

        t.SetTarget(target);
        return t;
    }
}
