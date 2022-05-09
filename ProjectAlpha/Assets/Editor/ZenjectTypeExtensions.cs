using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Code.Editor;

public static class ZenjectTypeExtensions
{
    private static bool DerivesFromOrEqual(this Type a, Type b)
    {
#if UNITY_WSA && ENABLE_DOTNET && !UNITY_EDITOR
            return b == a || b.GetTypeInfo().IsAssignableFrom(a.GetTypeInfo());
#else
        return b == a || b.IsAssignableFrom(a);
#endif
    }

    public static bool HasAttribute<T>(this MemberInfo provider)
        where T : Attribute
    {
        return provider.AllAttributes(typeof(T)).Any();
    }

    private static IEnumerable<Attribute> AllAttributes(
        this MemberInfo provider, params Type[] attributeTypes)
    {
        Attribute[] allAttributes;
#if NETFX_CORE
            allAttributes = provider.GetCustomAttributes<Attribute>(true).ToArray();
#else
        allAttributes = Attribute.GetCustomAttributes(provider, typeof(Attribute), true);
#endif
        if (attributeTypes.Length == 0)
        {
            return allAttributes;
        }

        return allAttributes.Where(a => attributeTypes.Any(x => a.GetType().DerivesFromOrEqual(x)));
    }
}