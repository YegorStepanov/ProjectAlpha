using System;
using UnityEngine;

namespace Code.Annotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NotNull : ValidationAttribute
    {
        public override string ErrorMessage => error;
        private string error = string.Empty;

        public override bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance)
        {
            bool isValid;
            MonoBehaviour mb = instance as MonoBehaviour;
            error = $"Property: {field.Name}\non GameObject: {mb.name}\ncannot be NULL";
            try
            {
                var value = field.GetValue(instance);
                isValid = !(value.Equals(null));
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}