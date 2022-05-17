using System;

namespace Code;

public static class TypeExtensions
{
    public static bool IsAssignableTo(this Type givenType, Type anotherType) =>
        anotherType.IsAssignableFrom(givenType);

    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        Type[] interfaceTypes = givenType.GetInterfaces();

        foreach (Type it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        Type baseType = givenType.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}