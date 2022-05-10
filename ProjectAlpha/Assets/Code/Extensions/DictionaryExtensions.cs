using System.Collections.Generic;

namespace Code;

public static class DictionaryExtensions
{
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> tuple, out TKey key, out TValue value)
    {
        key = tuple.Key;
        value = tuple.Value;
    }

    public static TValue Pop<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        TValue value = dictionary[key];
        dictionary.Remove(key);
        return value;
    }
}