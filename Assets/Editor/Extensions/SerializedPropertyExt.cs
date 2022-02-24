using System;
using System.Reflection;
using UnityEditor;

namespace UnrealEditor.Extensions
{
    public static class SerializedPropertyExt
    {
        private static readonly BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static FieldInfo GetFieldInfo(this SerializedProperty property, out object targetObject)
        {
            string[] components = property.propertyPath.Replace("Array.data[", "[").Split('.');

            targetObject = property.serializedObject.targetObject;
            FieldInfo fieldInfo = null;
            for (int index = 0; index < components.Length; index++)
            {
                string name = components[index];

                if (targetObject is Array array)
                {
                    targetObject = GetArrayValue(array, name);

                    continue;
                }

                fieldInfo = GetField(targetObject, name);

                if (index == components.Length - 1) continue;

                targetObject = fieldInfo.GetValue(targetObject);
            }

            return fieldInfo;
        }

        public static void SetValue<TValue>(this SerializedProperty property, TValue value)
        {
            string[] components = property.propertyPath.Replace("Array.data[", "[").Split('.');

            object targetParentObject = property.serializedObject.targetObject;
            FieldInfo targetObjectFieldInfo = null;
            for (int index = 0; index < components.Length; index++)
            {
                string name = components[index];

                if (targetParentObject is Array array)
                {
                    targetParentObject = GetArrayValue(array, name);

                    continue;
                }
                else
                {
                    targetObjectFieldInfo = GetField(targetParentObject, name);
                }

                if (name == property.name)
                {
                    break;
                }

                targetParentObject = targetObjectFieldInfo.GetValue(targetParentObject);
            }

            targetObjectFieldInfo.SetValue(targetParentObject, value);
        }

        private static FieldInfo GetField(object @object, string name)
        {
            Type type = @object.GetType();
            while (type != null)
            {
                FieldInfo fieldInfo = type.GetField(name, _bindingFlags);
                if (fieldInfo != null)
                {
                    return fieldInfo;
                }

                type = type.BaseType;
            }
            return null;
        }

        private static object GetArrayValue(Array array, string name)
        {
            int index = int.Parse(name.Replace("[", "").Replace("]", ""));
            return array.GetValue(index);
        }
    }
}
