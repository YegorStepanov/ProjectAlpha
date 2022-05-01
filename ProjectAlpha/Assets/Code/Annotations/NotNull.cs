using System;
using UnityEngine;

namespace Code.Annotations;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class NotNull : ValidationAttribute
{
    private string _error = string.Empty;
    public override string ErrorMessage => _error;

    public override bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance)
    {
        bool isValid;
        MonoBehaviour mb = instance as MonoBehaviour;
        _error = $"Property: {field.Name}\non GameObject: {mb.name}\ncannot be NULL";
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