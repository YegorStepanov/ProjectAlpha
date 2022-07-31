using Code.Common;
using Code.Data.PositionGenerator;
using Code.Extensions;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public class CameraMover
{
    private readonly ICamera _camera1;
    private readonly PlatformPositionGenerator _platformPositionGenerator;
    private readonly CherryPositionGenerator _cherryPositionGenerator;

    public CameraMover(ICamera camera1, PlatformPositionGenerator platformPositionGenerator, CherryPositionGenerator cherryPositionGenerator)
    {
        _camera1 = camera1;
        _platformPositionGenerator = platformPositionGenerator;
        _cherryPositionGenerator = cherryPositionGenerator;
    }

    public Borders GetNextCameraBorders(IPlatform currentPlatform)
    {
        Vector2 offset = currentPlatform.Borders.LeftBot - _camera1.Borders.LeftBot;
        return _camera1.Borders.Shift(offset);
    }

    //destination instead next
    public async UniTask MoveCamera(Borders nextCameraBorders, IPlatform currentPlatform, IPlatform nextPlatform, ICherry nextCherry)
    {
        //split to MoveEntities/MovePlatformAndCherry?
        float platformDestinationX = _platformPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, nextCameraBorders.Right, nextPlatform.Borders.Width);

        float cherryDestinationX = _cherryPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, platformDestinationX - nextPlatform.Borders.HalfWidth, nextCherry.Borders.Width);

        await UniTask.WhenAll(
            _camera1.MoveAsync(nextCameraBorders.Center),
            nextPlatform.MoveAsync(platformDestinationX),
            nextCherry.MoveAsync(cherryDestinationX));
    }
}
