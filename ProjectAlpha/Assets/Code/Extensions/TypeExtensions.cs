using System;

namespace Code.Extensions;

public static class TypeExtensions
{
    public static bool IsAssignableTo(this Type givenType, Type anotherType) =>
        anotherType.IsAssignableFrom(givenType);
}
