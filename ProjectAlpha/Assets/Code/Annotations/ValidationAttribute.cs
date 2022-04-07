using System.Reflection;

namespace Code.Annotations
{
    public abstract class ValidationAttribute : OneByOneAttribute
    {
        public abstract bool Validate(FieldInfo field, UnityEngine.Object instance);
        public abstract string ErrorMessage { get; }
    }
}