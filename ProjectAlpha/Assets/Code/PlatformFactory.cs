using DG.Tweening;
using FluentAssertions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code
{
    public sealed class PlatformFactory : SerializedMonoBehaviour
    {
        public Sprite s;

        [Required, SerializeField, AssetSelector(Filter = "t:PlatformWidthGenerator")]
        private PlatformWidthGenerator platformWidthGenerator;

        private CameraService cameraService;
        private PlatformFactory1 platformFactory;

        [SerializeField] private SpriteRenderer SpriteRendererPrefab;

        [SerializeField] private GameSettings gameSettings;

        private void Awake()
        {
            DOTween.Init();

            Camera main = Camera.main;

            platformWidthGenerator.Reset();

            cameraService = new CameraService(main, gameSettings);
            cameraService.Init();
            platformFactory = new PlatformFactory1(cameraService, platformWidthGenerator, SpriteRendererPrefab,
                gameSettings);
        }

        [Button]
        public void PlayAnimation()
        {
            cameraService.Move();
        }

        void Start()
        {
            platformFactory.creator.CreateMenuPlatform();
            platformFactory.creator.CreatePlatform();
        }
    }
}

//public record Borders(float Top, float Bottom, float Left, float Right);

/*
// public class GameBordersService
// {
//     private float startPlatformHeight = 0.2f;
//     private float gamePlatformHeight = 0.3f;
//
//     //private float CameraHeightOffset;
//
//     //private Borders borders;
//
//     private readonly ScreenService screenService;
//
//     public GameBordersService(ScreenService screenService)
//     {
//         this.screenService = screenService;
//         Initialize();
//     }
//
//     public void Initialize()
//     {
//         // CameraHeightOffset = gamePlatformHeight - startPlatformHeight;
//
//         // borders = new Borders(
//         //     (1 + CameraHeightOffset) * screenService.TopBorder(),
//         //     (1 + CameraHeightOffset) * screenService.BottomBorder(),
//         //     (1 + CameraHeightOffset) * screenService.LeftBorder(),
//         //     (1 + CameraHeightOffset) * screenService.RightBorder());
//     }
// }
*/