using Sirenix.OdinInspector;
using UnityEngine;

namespace Code;

[System.Serializable]
public sealed class GameSettings
{
    [SerializeField, LabelText("DelayBeforeNextRound (s)")]
    private float _delayBeforeNextRound = 0.1f;
    [SerializeField, LabelText("DelayBeforeEndGame (s)")]
    private float _delayBeforeEndGame = 0.3f;
    [SerializeField, LabelText("DelayBeforeHeroMovement (s)")]
    private float _delayBeforeHeroMovement = 0.2f;
    [SerializeField, LabelText("DelayBeforeFalling (s)")]
    private float _delayBeforeFalling = 0.1f;
    [SerializeField, LabelText("DelayBeforeFalling (s)")]
    private float _delayBeforeRotatingStickDown = 0.1f;

    public int DelayAfterHeroMovementToPlatform => (int)(_delayBeforeNextRound * 1000);
    public int DelayBeforeEndGameInEndGame => (int)(_delayBeforeEndGame * 1000);
    public int DelayBeforeHeroMovement => (int)(_delayBeforeHeroMovement * 1000);
    public int DelayBeforeFallingInEndGame => (int)(_delayBeforeFalling * 1000);
    public int DelayBeforeRotatingStickDownInEndGame => (int)(_delayBeforeRotatingStickDown * 1000);

    [Space]
    public float ViewportMenuPlatformPositionX = 0.5f;
    public float MenuPlatformWidth = 2f;
    [MinValue(0), MaxValue(1)]
    public float ViewportHeight = 0.2f;
    [MinValue(-1), MaxValue(1)]
    public float ViewportCameraMovementYOnGameStart = 0.1f; //bug when value < 0

    [Space]
    [Range(0, 1)]
    public float CherryChance = 0.1f;
}
