using System;
using Code.Common;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Game.States
{
    public interface IHeroController
    {
         UniTaskVoid MoveToAsync(Vector3 destination);
         void TeleportTo(Vector2 destination, HeroPivot relativeTo);
    }
    
    public enum HeroPivot
    {
        BottomRight,
        BottomCenter
    }

    public sealed class HeroController : MonoBehaviour, IHeroController
    {
        [field: SerializeField] private SpriteRenderer spriteRenderer;
        [field: SerializeField] private Transform bottomRightPivot;

        public async UniTaskVoid MoveToAsync(Vector3 destination) =>
            await transform.DOMoveX(5, 10).SetSpeedBased(true);

        public void TeleportTo(Vector2 destination, HeroPivot relativeTo)
        {
            Vector2 endPoint = relativeTo switch
            {
                HeroPivot.BottomRight => destination,
                HeroPivot.BottomCenter => OffsetBottomCenterPivot(destination),
                _ => throw new ArgumentOutOfRangeException(nameof(relativeTo), relativeTo, null)
            }; 
            
            TeleportTo(endPoint);
        }

        private void TeleportTo(Vector2 destination) =>
            transform.position = destination;

        private Vector2 OffsetBottomCenterPivot(Vector2 destination)
        {
            Vector2 size = SpriteHelper.WorldSpriteSize(spriteRenderer.sprite, transform.lossyScale);
            return destination.ShiftX(size.x / 2f);
        }
    }
}