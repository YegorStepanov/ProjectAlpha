using Code.Services.Infrastructure;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Extensions;

[PublicAPI]
// ReSharper disable once InconsistentNaming
public static class ICameraExtensions
{
    public static Vector2 ViewportToWorldPosition<T>(this T camera, Vector2 viewportPosition) where T : ICamera =>
        new(camera.ViewportToWorldPosX(viewportPosition.x),
            camera.ViewportToWorldPosY(viewportPosition.y));

    public static float ViewportToWorldPosX<T>(this T camera, float viewportPosX) where T : ICamera =>
        camera.Borders.Left + camera.ViewportToWorldWidth(viewportPosX);

    public static float ViewportToWorldPosY<T>(this T camera, float viewportPosY) where T : ICamera =>
        camera.Borders.Bot + camera.ViewportToWorldHeight(viewportPosY);

    public static Vector2 ViewportToWorldSize<T>(this T camera, Vector2 viewportSize) where T : ICamera =>
        new(camera.ViewportToWorldWidth(viewportSize.x),
            camera.ViewportToWorldHeight(viewportSize.y));

    public static float ViewportToWorldWidth<T>(this T camera, float viewportWidth) where T : ICamera =>
        camera.Borders.Width * viewportWidth;

    public static float ViewportToWorldHeight<T>(this T camera, float viewportHeight) where T : ICamera =>
        camera.Borders.Height * viewportHeight;
}
