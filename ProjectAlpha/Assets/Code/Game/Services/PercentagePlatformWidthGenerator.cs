using Code.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game
{
    [CreateAssetMenu(menuName = "SO/Platform Width Generator")]
    public class PercentagePlatformWidthGenerator : PlatformWidthGenerator
    {
        // [SerializeField, MinMaxSlider(0, 1), LabelText("Range of the platform")]
        // private Vector2 platformViewportRange = new(0.1f, 0.5f);
        //
        // [SerializeField, ProgressBar(0, 100), LabelText("Reduction per step (%)")]
        // private float PercentageReductionPerStep = 10f;
        //
        // [SerializeField, MinMaxSlider(0, 1), LabelText("Min/Max threshold")]
        // private Vector2 minMaxViewportThreshold = new(0.1f, 0.2f);

        [SerializeField]
        private float MinWidth = 0.25f;

        [SerializeField]
        private float MaxWidth = 2f;

        [SerializeField, Range(0, 1)]
        private float ReductionRatioPerStep = 0.1f;

        [SerializeField]
        private float MinThreshold = 0.25f;

        [SerializeField]
        private float MaxThreshold = 2f;
        
        private Ratio currentRatio;

        public override void Reset() =>
            currentRatio = new Ratio(1f, 1f);

        public override float NextWidth()
        {
            float width = NextWidth(currentRatio);
            currentRatio = NextRatio(currentRatio);
            currentRatio = LimitByThreshold(currentRatio);
            return width;
        }

        private float NextWidth(Ratio ratio) =>
            Random.Range(MinWidth * ratio.Min, MaxWidth * ratio.Max);

        private Ratio NextRatio(Ratio ratio) =>
            ratio * (1f - ReductionRatioPerStep);

        private Ratio LimitByThreshold(Ratio ratio) => new(
            ratio.Min < MinThreshold ? MinThreshold : ratio.Min,
            ratio.Max < MaxThreshold ? MaxThreshold : ratio.Max);

        
    }
}