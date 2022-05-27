using UnityEngine;

namespace Code.AddressableAssets;

public readonly record struct Address<T>(string Key) where T : Object
{
    public Address<TResult> As<TResult>() where TResult : Object =>
        new(Key);

    public AddressData AsData() =>
        new(Key, typeof(T));
}