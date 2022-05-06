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
    
    public static T ResolveInstance<T, TParam>(this IObjectResolver resolver, TParam parameter)
    {
        var registrationBuilder = new RegistrationBuilder(typeof(T), Lifetime.Transient);
        
        if(parameter != null)
            registrationBuilder = registrationBuilder.WithParameter(parameter);
    
        Registration registration = registrationBuilder.Build();
        return (T)resolver.Resolve(registration);
    }
}