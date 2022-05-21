using Code.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class WidthGeneratorSpawner
{
    private readonly IAsyncObject<WidthGeneratorData> _widthGenerator;

    public WidthGeneratorSpawner(IAsyncObject<WidthGeneratorData> widthGenerator) =>
        _widthGenerator = widthGenerator;

    public async UniTask<IWidthGenerator> GetAsync()
    {
        WidthGeneratorData stick = await _widthGenerator.GetAssetAsync();
        return stick.Create(); //todo: cache it
    }
}