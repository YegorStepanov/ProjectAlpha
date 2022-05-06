using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly LifetimeScope _scope;
    private readonly IObjectResolver _resolver;
    private readonly PlatformController _platformPrefab;
    private readonly CameraController _cameraController;

    public PlatformSpawner(
        LifetimeScope scope,
        IObjectResolver resolver,
        PlatformController platformPrefab,
        CameraController cameraController)
    {
        _scope = scope;
        _resolver = resolver;
        _platformPrefab = platformPrefab;
        _cameraController = cameraController;
    }

    public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
    {
        PlatformController platform = _resolver.Instantiate(_platformPrefab, _scope.transform);
        platform.transform.SetParent(null);

        float height = _cameraController.Borders.TransformPointY(position.y, Relative.Bottom);

        Vector2 size = new(width, height);
        platform.SetSize(size);

        position = platform.Borders.TransformPoint(position, relative);
        platform.SetPosition(position);

        return platform;
    }
}