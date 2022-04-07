using System;
using System.Collections;
using UnityEngine;

namespace Code.Annotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ContainsAtLeast : ValidationAttribute
    {
        public override string ErrorMessage => error;
        private string error = string.Empty;

        private int Count;

        public ContainsAtLeast(int minimumItemCount)
        {
            Count = minimumItemCount;
        }

        public override bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance)
        {
            var value = field.GetValue(instance);
            var mb = instance as MonoBehaviour;
            var isValid = false;
            try
            {
                error = $"Object: {field.Name}\non GameObject: {mb.name}\nMust contain AT LEAST ({Count}) item(s)";
                int i = 0;
                foreach (var o in (value as IEnumerable))
                {
                    i++;
                    if (i >= Count)
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
                    error = $"Object: {field.Name}\non GameObject: {mb.name}\nMust contain AT LEAST ({Count}) item(s)";
                else
                    error =
                        $"Item: {field.Name}\non GameObject: {mb.name}\nMust be Array, List, or String to use attribute properly";
            }

            return isValid;
        }
    }
}