namespace Code.AddressableAssets.Factories;

public interface IFactory<out TValue> //perf?
{
    TValue Create();
}
