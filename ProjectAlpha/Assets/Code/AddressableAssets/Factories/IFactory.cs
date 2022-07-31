namespace Code.AddressableAssets;

public interface IFactory<out TValue> //perf?
{
    TValue Create();
}
