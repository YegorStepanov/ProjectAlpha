namespace Code.Extensions;

public static class UnityExtensions
{
    // ReSharper disable once CompareNonConstrainedGenericWithNull
    public static bool IsUnityNull<T>(this T obj) => obj == null || obj.Equals(null);
}
