namespace Code.AddressableAssets
{
//check performance of covariance modifier
    public interface IFactory<out TValue>
    {
        TValue Create();
    }
}