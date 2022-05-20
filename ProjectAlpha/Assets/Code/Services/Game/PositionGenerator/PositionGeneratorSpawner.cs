using Code.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class PositionGeneratorSpawner
{
    private readonly IAsyncObject<PositionGeneratorData> _widthGenerator;

    public PositionGeneratorSpawner(IAsyncObject<PositionGeneratorData> widthGenerator) =>
        _widthGenerator = widthGenerator;

    public async UniTask<IPositionGenerator> GetAsync()
    {
        PositionGeneratorData data = await _widthGenerator.GetAssetAsync();
        return data.Create();
    }
}