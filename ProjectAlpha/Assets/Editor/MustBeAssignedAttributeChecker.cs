using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Editor
{
    public class MyEditorEvents : UnityEditor.AssetModificationProcessor
    {
        /// <summary>
        /// Occurs on Scenes/Assets Save
        /// </summary>
        public static event Action OnSave;

        /// <summary>
        /// On Editor Save
        /// </summary>
        private static string[] OnWillSaveAssets(string[] paths)
        {
            // Prefab creation enforces SaveAsset and this may cause unwanted dir cleanup
            if (paths.Length == 1 && (paths[0] == null || paths[0].EndsWith(".prefab"))) return paths;

            OnSave?.Invoke();

            return paths;
        }
    }

    public static class MyScriptableObject
    {
        /// <summary>
        /// Load all SO of type from Assets
        /// </summary>
        public static T[] LoadAssets<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            var assets = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return assets;
        }
    }

    [InitializeOnLoad]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public class MustBeAssignedAttributeChecker
    {
        private static readonly string[] excludedTypes =
        {
            "TMPro.TMP_FontAsset",
            "TMPro.TMP_Settings",
            "UnityEditor.AddressableAssets.Settings.AddressableAssetGroup",
            "UnityEditor.AddressableAssets.Settings.AddressableAssetGroupTemplate",
            "UnityEngine.EventSystems.EventSystem",
            "UnityEngine.GUISkin",
            "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset",
            "UnityEngine.Rendering.Universal.UniversalAdditionalCameraData"
        };

        private static readonly (string, string)[] excludedPropertyInType =
        {
            ("UnityEngine.UI.Image", "m_Material")
        };

        static MustBeAssignedAttributeChecker()
        {
            MyEditorEvents.OnSave += AssertComponentsInScene;
            PrefabStage.prefabSaved += AssertComponentsInPrefab;
        }

        private static bool IsFieldExcludedByType(Object obj)
        {
            string name = obj.GetType().FullName;

            foreach (string excludedType in excludedTypes)
            {
                if (excludedType == name) return true;
            }

            return false;
        }

        private static bool IsFieldExcludedByProperty(FieldInfo field, Object obj)
        {
            //im not sure what I wrote
            string typeName = obj.GetType().FullName;
            string fieldName = field.Name;
            
            foreach ((string excludedType, string excludedProperty) in excludedPropertyInType)
            {
                if (fieldName == excludedProperty && excludedType == typeName)
                {
                    return true;
                }
            }

            return false;
        }

        private static void AssertComponentsInScene()
        {
            MonoBehaviour[] monoBehaviours = Object.FindObjectsOfType<MonoBehaviour>(true);
            AssertComponents(monoBehaviours);

            ScriptableObject[] scriptableObjects = MyScriptableObject.LoadAssets<ScriptableObject>();
            AssertComponents(scriptableObjects);
        }

        private static void AssertComponentsInPrefab(GameObject prefab)
        {
            MonoBehaviour[] monoBehaviours = prefab.GetComponentsInChildren<MonoBehaviour>();
            AssertComponents(monoBehaviours);
        }

        private static void AssertComponents(Object[] objects)
        {
            foreach (Object obj in objects)
            {
                if (obj == null) continue;

                Type typeOfScript = obj.GetType();

                IEnumerable<FieldInfo> inspectedFields = GetInspectedFields(obj);
                IEnumerable<FieldInfo> nullFields = inspectedFields.Where(f => IsNull(f, obj));

                foreach (FieldInfo field in nullFields)
                {
                    if (IsFieldExcluded(field, obj)) continue;

                    AssertField(obj, typeOfScript, field);
                }
            }
        }

        private static IEnumerable<FieldInfo> GetInspectedFields(Object component)
        {
            Type type = component.GetType();
            List<FieldInfo> inspectedFields = new();

            List<FieldInfo> publicFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToList();
            List<FieldInfo> privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();

            //Private fields can be inspected if they are explicitly serialized
            privateFields = privateFields.Where(f => f.HasAttribute<SerializeField>()).ToList();
            //Add remaining private and public fields to the list of all inspectable fields
            inspectedFields.AddRange(publicFields);
            inspectedFields.AddRange(privateFields);
            //Remove fields that should be hidden in the inspector
            inspectedFields = inspectedFields.Where(f => !f.HasAttribute<HideInInspector>()).ToList();

            return inspectedFields;
        }

        private static bool IsNull(FieldInfo field, object obj)
        {
            object value = field.GetValue(obj);
            return value == null || value.ToString() == "null";
        }

        private static void AssertField(Object targetObject, Type targetType, FieldInfo field)
        {
            object fieldValue = field.GetValue(targetObject);

            bool valueTypeWithDefaultValue = field.FieldType.IsValueType &&
                                             Activator.CreateInstance(field.FieldType).Equals(fieldValue);
            if (valueTypeWithDefaultValue)
            {
                Debug.LogError(
                    $"{targetType.Name} caused: {field.Name} is Value Type with default value. Class: {targetObject.GetType().FullName}",
                    targetObject);
                return;
            }


            bool nullReferenceType = fieldValue == null || fieldValue.Equals(null);
            if (nullReferenceType)
            {
                Debug.LogError(
                    $"{targetType.Name} caused: {field.Name} is not assigned (null value). Class: {targetObject.GetType().FullName}",
                    targetObject);
                return;
            }


            bool emptyString = field.FieldType == typeof(string) && (string)fieldValue == string.Empty;
            if (emptyString)
            {
                Debug.LogError(
                    $"{targetType.Name} caused: {field.Name} is not assigned (empty string). Class: {targetObject.GetType().FullName}",
                    targetObject);
                return;
            }


            bool emptyArray = fieldValue is Array arr && arr.Length == 0;
            if (emptyArray)
            {
                Debug.LogError(
                    $"{targetType.Name} caused: {field.Name} is not assigned (empty array). Class: {targetObject.GetType().FullName}",
                    targetObject);
            }
        }

        private static bool IsFieldExcluded(FieldInfo field, Object obj)
        {
            if (IsFieldExcludedByType(obj)) return true;
            if (IsFieldExcludedByProperty(field, obj)) return true;
            return false;
        }
    }
}