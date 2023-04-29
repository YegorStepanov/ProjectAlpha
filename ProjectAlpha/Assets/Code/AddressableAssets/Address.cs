using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Code.AddressableAssets
{
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public readonly record struct Address<T>(string Key) where T : Object
    {
        public Address<TResult> As<TResult>() where TResult : Object =>
            new(Key);
    }
}