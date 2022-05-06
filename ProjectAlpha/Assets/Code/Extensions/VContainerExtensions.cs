using VContainer;

namespace Code;

public static class VContainerExtensions
{
    public static T ResolveInstance<T>(this IObjectResolver resolver)
    {
        var registrationBuilder = new RegistrationBuilder(typeof(T), Lifetime.Transient);
        
        Registration registration = registrationBuilder.Build();
        return (T)resolver.Resolve(registration);
    }
}