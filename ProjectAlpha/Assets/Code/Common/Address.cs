using UnityEngine;

namespace Code;

public readonly record struct Address<T>(string Key) where T : Object
{
    public Address<TResult> As<TResult>() where TResult : Object =>
        new(Key);

    public AddressData AsData<TResult>() where TResult : Object =>
        new(Key, typeof(T));
}