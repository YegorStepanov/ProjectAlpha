using System.Reflection;

namespace Code.Annotations;

public abstract class ValidationAttribute : OneByOneAttribute
{
    public abstract string ErrorMessage { get; }
    public abstract bool Validate(FieldInfo field, UnityEngine.Object instance);
}