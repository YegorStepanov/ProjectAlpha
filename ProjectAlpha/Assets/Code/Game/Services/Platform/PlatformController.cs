using Code.Common;
using Code.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public interface IPlatformController
    {
        void Construct(CameraService cameraService);
        void SetPosition(Vector2 position);
        void SetSize(Vector2 scale);
        Vector2 Position { get; }
        
        Borders Borders { get; }
    }
    
    public sealed class PlatformController : MonoBehaviour, IPlatformController
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private CameraService cameraService;

        public Vector2 Position => transform.position;

        public Borders Borders
        {
            get
            {
                Bounds bounds = spriteRenderer.bounds;
                return new Borders(bounds.max.y, bounds.min.y, bounds.min.x, bounds.max.x);
            }
        }

        [Inject]
        public void Construct(CameraService _cameraService)
        {
            cameraService = _cameraService;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetSize(Vector2 scale)
        {
            Vector2 spriteSize = spriteRenderer.bounds.size;
            transform.localScale = scale / spriteSize;
            Debug.Log(spriteRenderer.bounds.min);
            Debug.Log(spriteRenderer.bounds.max);
            Debug.Log(transform.position + spriteRenderer.sprite.bounds.min);
            Debug.Log(transform.position + spriteRenderer.sprite.bounds.max);
        }
        
        public class Pool : MonoMemoryPool<PlatformController> { }
    }
}