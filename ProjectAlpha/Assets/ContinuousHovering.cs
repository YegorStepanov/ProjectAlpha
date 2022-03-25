using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ContinuousHovering : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    
    [SerializeField] private float hoveringRange;

    [SerializeField] private float duration;

    private float HalfOfRange =>
        hoveringRange / 2;

    private async UniTaskVoid Start()
    {
        await MoveToAnimationStartAsync();
        StartCyclicAnimation();
    }

    private UniTask MoveToAnimationStartAsync() => 
        rectTransform.DOAnchorPosY(-HalfOfRange, duration / 2).ToUniTask();

    private void StartCyclicAnimation() =>
        rectTransform.DOAnchorPosY(HalfOfRange, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
}