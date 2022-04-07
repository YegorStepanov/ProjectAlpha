#if UNITY_EDITOR
using System;
using System.Linq;
using OdinExtensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
using Sirenix.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: RegisterValidator(typeof(AllRequiredValidator), Priority = 500)]

namespace OdinExtensions
{
	/// <summary>
	/// Makes all fields that take an object to be REQUIRED by default.
	/// An 'OptionalAttribute' can be used to make objects optional.
	///
	/// Built-in unity types and other plugins things should not be included,
	/// as they can often introduce random errors since they were not designed
	/// with this in mind.
	///
	/// Use the whitelist to select namespaces to include.
	/// You can also use the blacklist to block subsections
	/// of the whitelist.
	/// </summary>
	public class AllRequiredValidator : Validator
	{
		public override RevalidationCriteria RevalidationCriteria => RevalidationCriteria.OnValueChange;
		// CUSTOMIZABLE WHITELIST (namespaces)
		// ----------------------------------------
		private static readonly string[] Whitelist =
		{
			// Fill me up
			"Code"
		};

		// CUSTOMIZABLE BLACKLIST (namespaces)
		// ----------------------------------------
		private static readonly string[] Blacklist =
		{
			// Fill me up
		};

		private static bool IsTypeSupported(Type type)
		{
			string typeNamespace = type.Namespace;

			if (typeNamespace == null)
				return false;


			return Whitelist.Any(w => typeNamespace.StartsWith(w)) &&
				   !Blacklist.Any(b => typeNamespace.StartsWith(b));
		}

		private static bool IsValid(IPropertyValueEntry valueEntry)
		{
			object v = valueEntry?.WeakSmartValue;

			if (v == null) return false;
			if (v is Object o && o == null) return false;
			if (v is string s && string.IsNullOrEmpty(s)) return false;

			return true;
		}

		public override void RunValidation(ref ValidationResult result)
		{
			if (Property.ValueEntry == null) return;
			if (Property.Name.StartsWith("$")) return;

			if (result == null)
			{
				result = new ValidationResult
				{
					Setup = new ValidationSetup
					{
						Root      = Property.SerializationRoot.ValueEntry.WeakValues[0] as Object,
						Member    = Property.Info.GetMemberInfo(),
						Validator = this
					},
					ResultType = ValidationResultType.Valid
				};
			}

			// Check to see that this property is allowed for AllRequired
			Type parentType = Property.ParentType;
			if (!IsTypeSupported(parentType))
				return;
			
			// Apply the result
			if (IsValid(Property.ValueEntry))
			{
				result.ResultType = ValidationResultType.Valid;
				result.Message    = null;
			}
			else
			{
				if (Property.GetAttribute<OptionalAttribute>() != null)
					return;

				result.ResultType = ValidationResultType.Error;
				result.Message    = $"'{Property.Name}' must be assigned. All public values are serialized in Unity. Values which are not meant for configuration must be marked with NonSerialized.";
			}
		}

		public override bool CanValidateProperty(InspectorProperty property)
		{
			bool is_public_field      = property.Info.IsEditable && property.Info.HasBackingMembers;
			bool serialized_attr      = property.GetAttribute<SerializeField>() != null;
			bool serialized_attr_odin = property.GetAttribute<OdinSerializeAttribute>() != null;

			return is_public_field || serialized_attr || serialized_attr_odin;
		}
	}
}
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OptionalAttribute : Attribute
{ }