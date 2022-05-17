using Code.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class WidthGeneratorSpawner
{
    private readonly IAsyncObject<WidthGenerator> _widthGenerator;

    public WidthGeneratorSpawner(IAsyncObject<WidthGenerator> widthGenerator) =>
        _widthGenerator = widthGenerator;

    public async UniTask<WidthGenerator> CreateAsync()
    {
        WidthGenerator stick = await _widthGenerator.GetAssetAsync();
        stick.Reset();
        return stick;
    }
}