namespace Code.VContainer;

public interface IFactory<out TValue> //perf?
{
    TValue Create();
}
