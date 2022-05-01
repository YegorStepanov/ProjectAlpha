using System;
using System.Collections;
using UnityEngine;

namespace Code.Annotations;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ContainsAtLeast : ValidationAttribute
{
    private readonly int _count;
    private string _error = string.Empty;

    public ContainsAtLeast(int minimumItemCount)
    {
        _count = minimumItemCount;
    }

    public override string ErrorMessage => _error;

    public override bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance)
    {
        var value = field.GetValue(instance);
        var mb = instance as MonoBehaviour;
        var isValid = false;
        try
        {
            _error = $"Object: {field.Name}\non GameObject: {mb.name}\nMust contain AT LEAST ({_count}) item(s)";
            int i = 0;
            foreach (var o in (value as IEnumerable))
            {
                i++;
                if (i >= _count)
                {
                    isValid = true;
                    break;
                }
            }
        }
        catch (Exception)
        {
            isValid = false;
            if (typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                _error = $"Object: {field.Name}\non GameObject: {mb.name}\nMust contain AT LEAST ({_count}) item(s)";
            else
                _error =
                    $"Item: {field.Name}\non GameObject: {mb.name}\nMust be Array, List, or String to use attribute properly";
        }

        return isValid;
    }
}