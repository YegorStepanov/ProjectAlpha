using Code;
using DG.Tweening;
using UnityEngine;

public sealed class CameraService //: ITickable
{
    private readonly Camera camera;
    private readonly GameSettings gameSettings;

    public Borders MenuBorders { get; private set; }
    public Borders GameBorders { get; private set; }

    public Vector2 ViewportGameCameraOffset = new(0f, -0.2f);

    public CameraService(Camera camera, GameSettings gameSettings)
    {
        this.camera = camera;
        this.gameSettings = gameSettings;
    }

    public void Init() =>
        UpdateBorders(); //TODO

    public void Move() //Transition from the Menu to the Game state
    {
        float offsetX = OffsetToLeftCameraBorder() + MenuPlatformOffsetToLeftBorder();
        float offsetY = MenuToGamePlatformOffsetY();

        var offset = new Vector3(offsetX, offsetY, camera.transform.position.z);
        
        camera.transform.DOMove(offset, gameSettings.MenuToGameCameraAnimationDuration);
    }

    private void UpdateBorders()
    {
        Vector2 topRightCorner = camera.ViewportToWorldPoint(Vector2.one);
        Vector2 bottomLeftCorner = camera.ViewportToWorldPoint(Vector2.zero);

        MenuBorders = new Borders(
            Top: topRightCorner.y,
            Right: topRightCorner.x,
            Bottom: bottomLeftCorner.y,
            Left: bottomLeftCorner.x
        );

        GameBorders = new Borders(
            Top: MenuBorders.Top,
            Right: MenuBorders.Right,
            Bottom: MenuBorders.Bottom,
            Left: MenuBorders.Left
        );
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
        camera.ViewportToWorldPoint(viewportPosition);

    public float WorldHeight() =>
        camera.orthographicSize * 2;

    public float WorldWidth() =>
        camera.orthographicSize * camera.aspect * 2;

    private float OffsetToLeftCameraBorder() =>
        -ViewportToWorldPosition(Vector2.zero).x;

    private float MenuPlatformOffsetToLeftBorder() =>
        -gameSettings.MenuPlatformWidth / 2f;

    private float MenuToGamePlatformOffsetY() =>
        gameSettings.GamePlatformPositionY - gameSettings.MenuPlatformPositionY;
}