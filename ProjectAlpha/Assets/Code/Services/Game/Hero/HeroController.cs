using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class HeroController : MonoBehaviour, IHeroController
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform bottomRightPivot;
    
    private Settings settings;

    public Borders Borders => spriteRenderer.bounds.AsBorders();

    public float HandOffset => settings.HandOffset; //remove it

    [Inject]
    public void Construct(Settings settings) => 
        this.settings = settings;

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, settings.MovementSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased();

    public async UniTask FellAsync() =>
        await transform.DOMoveY(settings.FallingDestination, -settings.FallingSpeed)
            .SetSpeedBased();

    public void TeleportTo(Vector2 destination, Relative relative) =>
        transform.position = Borders.TransformPoint(destination, relative);

    [Serializable]
    public class Settings
    {
        public float HandOffset = 0.25f;
        public float MovementSpeed = 4f;
        public float FallingSpeed = 9.8f;
        public float FallingDestination = -10f;
    }
}