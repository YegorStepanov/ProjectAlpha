using System.Collections.Generic;
using UnityEngine;

namespace Code.Extensions;

public static class DictionaryExtensions
{
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> tuple, out TKey key, out TValue value)
    {
        key = tuple.Key;
        value = tuple.Value;
    }

    public static TValue Pop<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        if(dictionary.ContainsKey(key))
            Debug.LogError($"The dictionary does not contain key:{key}");

        TValue value = dictionary[key];
        dictionary.Remove(key);
        return value;
    }
}
