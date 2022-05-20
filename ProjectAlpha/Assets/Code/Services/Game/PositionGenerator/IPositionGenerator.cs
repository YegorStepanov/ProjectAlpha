namespace Code.Services;

public interface IPositionGenerator
{ 
    float NextPosition(IPlatformController currentPlatform, IPlatformController nextPlatform);
}