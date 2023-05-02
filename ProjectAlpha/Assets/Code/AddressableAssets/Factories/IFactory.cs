namespace Code.AddressableAssets
{
    // Check performance of covariance modifier
    public interface IFactory<out TValue>
    {
        TValue Create();
    }
}