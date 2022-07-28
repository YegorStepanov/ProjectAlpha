using UnityEngine;

namespace Code.States;

[System.Serializable]
public class GameLoopSettings
{
    //todo: InspectorName not working, use Odin isntead
    [SerializeField, InspectorName("DelayBeforeNextRound (s)")]
    private float DelayBeforeNextRound_Seconds = 0.1f;
    [SerializeField, InspectorName("DelayBeforeEndGame (s)")]
    private float DelayBeforeEndGame_Seconds = 0.3f;
    [SerializeField, InspectorName("DelayBeforeHeroMovement (s)")]
    private float DelayBeforeHeroMovement_Seconds = 0.2f;
    [SerializeField, InspectorName("DelayBeforeFalling (s)")]
    private float DelayBeforeFalling_Seconds = 0.1f;

    public int DelayBeforeNextRound => (int)(DelayBeforeNextRound_Seconds * 1000);
    public int DelayBeforeEndGame => (int)(DelayBeforeEndGame_Seconds * 1000);
    public int DelayBeforeHeroMovement => (int)(DelayBeforeHeroMovement_Seconds * 1000);
    public int DelayBeforeFalling => (int)(DelayBeforeFalling_Seconds * 1000);
}
