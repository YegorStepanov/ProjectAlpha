using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Animations;

public sealed class FloatContinually : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private float hoveringRange;

    [SerializeField] private float duration;

    private float HalfRange => hoveringRange / 2;

    private async UniTaskVoid Awake()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();

        await MoveToAnimationStartAsync(token);
        StartCyclicAnimationAsync(token);
    }

    private UniTask MoveToAnimationStartAsync(CancellationToken token) =>
        rectTransform.DOAnchorPosY(-HalfRange, duration / 2)
            .SetRelative()
            .WithCancellation(token);

    private void StartCyclicAnimationAsync(CancellationToken token) =>
        rectTransform.DOAnchorPosY(2 * HalfRange, duration)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad).WithCancellation(token);
}