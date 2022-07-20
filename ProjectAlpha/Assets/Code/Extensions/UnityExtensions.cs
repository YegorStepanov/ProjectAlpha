﻿using System.Diagnostics.CodeAnalysis;

namespace Code;

public static class UnityExtensions
{
    [SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
    public static bool IsUnityNull<T>(this T obj) => obj == null || obj.Equals(null);
}
