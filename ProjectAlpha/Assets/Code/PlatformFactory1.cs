using Code.Extensions;
using Code.Game;
using UnityEngine;

namespace Code.Game1
{
    public sealed class PlatformCreator
    {
        private const float viewportPlatformPositionX = 1f;

        private readonly SpriteRenderer spriteRendererPrefab;
        private readonly CameraService cameraService;
        private readonly PlatformWidthGenerator platformWidthGenerator;
        private readonly GameSettings gameSettings;

        public PlatformCreator(
            SpriteRenderer spriteRendererPrefab,
            CameraService cameraService,
            PlatformWidthGenerator platformWidthGenerator,
            GameSettings gameSettings)
        {
            this.spriteRendererPrefab = spriteRendererPrefab;
            this.cameraService = cameraService;
            this.platformWidthGenerator = platformWidthGenerator;
            this.gameSettings = gameSettings;
        }

        public void CreateMenuPlatform() =>
            CreatePlatform(gameSettings.MenuPlatformViewportPosition, gameSettings.MenuPlatformWidth);

        public void CreatePlatform()
        {
            float width = platformWidthGenerator.NextWidth();

            Transform platform = CreatePlatform(viewportPlatformPositionX, width);

            platform.position = MoveToLeftBorder(platform.position, width);
        }

        private static Vector2 MoveToLeftBorder(Vector2 position, float width) =>
            position.WithX(position.x + width / 2);

        public Transform CreatePlatform(float viewportPositionX, float width)
        {
            (Transform platform, SpriteRenderer renderer) = CreatePlatformObject();

            platform.position = ViewportToWorldPosition(viewportPositionX, gameSettings.MenuPlatformPositionY);

            Vector2 viewportGamePosition = ViewportGamePosition(platform.position);
            platform.localScale = Scale(viewportGamePosition, width, renderer.bounds.size);

            return platform;
        }

        private (Transform platform, SpriteRenderer renderer) CreatePlatformObject()
        {
            SpriteRenderer renderer = Object.Instantiate(spriteRendererPrefab);
            return (renderer.transform, renderer);
        }

        private Vector2 ViewportToWorldPosition(float x, float y) =>
            cameraService.ViewportToWorldPosition(new Vector2(x, y));

        private Vector2 ViewportGamePosition(Vector2 position) =>
            position - cameraService.ViewportToWorldPosition(cameraService.ViewportGameCameraOffset);

        private Vector2 Scale(Vector2 viewportPosition, float worldWidth, Vector2 spriteSize) => new(
            worldWidth / spriteSize.x,
            ScaleY(viewportPosition, spriteSize));

        // cameraService.WorldWidth() * viewportWidth / spriteSize.x;

        private float ScaleY(Vector2 viewportPosition, Vector2 spriteSize)
        {
            float platformHeight = Mathf.Abs(viewportPosition.y - cameraService.GameBorders.Bottom);
            return platformHeight / spriteSize.y;
        }

        // private static Vector2 OffsetToLeftBorder(Transform platform, Sprite sprite)
        // {
        //     Vector2 worldSize = SpriteHelper.WorldSpriteSize(sprite, platform.lossyScale);
        //     float distanceToBorder = worldSize.x / 2f;
        //
        //     Vector3 pos = platform.position;
        //     return pos.WithX(pos.x + distanceToBorder);
        // }
    }

    // public sealed class PlatformFactory1
    // {
    //     private readonly CameraService cameraService;
    //     private readonly PlatformWidthGenerator platformWidthGenerator;
    //     private readonly GameSettings settings;
    //
    //     public readonly PlatformCreator creator;
    //
    //     public PlatformFactory1(
    //         CameraService cameraService,
    //         PlatformWidthGenerator platformWidthGenerator,
    //         SpriteRenderer spriteRendererPrefab,
    //         GameSettings settings)
    //     {
    //         this.cameraService = cameraService;
    //         this.platformWidthGenerator = platformWidthGenerator;
    //         this.settings = settings;
    //
    //         creator = new PlatformCreator(spriteRendererPrefab, cameraService, platformWidthGenerator, settings);
    //     }
    // }
}