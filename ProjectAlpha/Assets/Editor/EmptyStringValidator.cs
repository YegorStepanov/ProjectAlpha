using System;
using System.Reflection;
using Code.Utils;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Object = UnityEngine.Object;

//[assembly: RegisterValidator(typeof(EmptyStringValidator))]

// [assembly: RegisterValidator(typeof(AA), Priority = 10_000)]

namespace Code.Utils
{
    public class AA : AttributeValidator<RequiredAttribute>
    {
        protected override void Validate(ValidationResult result)
        {
            base.Validate(result);
        }
    }

    public class EmptyStringValidator : ValueValidator<string>
    {
        protected override void Validate(ValidationResult result)
        {
            //Debug.Log("EmptyStringValidator:" + ValueEntry.SmartValue);
            if (string.IsNullOrEmpty(this.ValueEntry.SmartValue))
            {
                result.ResultType = ValidationResultType.Warning;
                result.Message = "This string is empty! Are you sure that's correct?";
            }
        }
    }
    //
    // [AttributeUsage(AttributeTargets.Field)]
    // public class RequiredOnSceneAttribute : Attribute
    // {
    //     // ReSharper disable once UnassignedField.Global
    //     public string ErrorMessage;
    // }
    //
    // public class RequiredOnSceneAttributeValidator : AttributeValidator<RequiredOnSceneAttribute>
    // {
    //     private StringMemberHelper stringHelper;
    //
    //     public override void Initialize(MemberInfo member, Type memberValueType)
    //     {
    //         if (this.Attribute.ErrorMessage != null)
    //         {
    //             this.stringHelper = new StringMemberHelper(member.ReflectedType, false, this.Attribute.ErrorMessage);
    //         }
    //     }
    //     
    //     protected override void Validate(object parentInstance, object memberValue, MemberInfo member, ValidationResult result)
    //     {
    //         if (!(parentInstance is MonoBehaviour)) return;
    //         
    //         MonoBehaviour parent = (MonoBehaviour) parentInstance;
    //         GameObject sourceObject = parent.gameObject;
    //         
    //         bool isInPrefabMode = PrefabStageUtility.GetPrefabStage(sourceObject) != null;
    //         bool memberValid = IsValid(memberValue);
    //         var scene = sourceObject.scene;
    //         bool validScene = scene.IsValid() && scene.isLoaded;
    //         bool valid = isInPrefabMode || memberValid || !validScene;
    //
    //         if (valid) return;
    //         result.ResultType = ValidationResultType.Error;
    //         result.Message = stringHelper != null ? stringHelper.GetString(parentInstance) : member.Name+" is required on scene!";
    //     }
    //     
    //     private static bool IsValid(object memberValue)
    //     {
    //         switch (memberValue)
    //         {
    //             case null:
    //             case string value when string.IsNullOrEmpty(value):
    //             case Object o when o == null:
    //                 return false;
    //             default:
    //                 return true;
    //         }
    //     }
    // }
}