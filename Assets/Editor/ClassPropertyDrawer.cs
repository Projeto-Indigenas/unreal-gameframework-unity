using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.Core;
using UnrealEditor.Extensions;

namespace UnrealEditor
{
    [CustomPropertyDrawer(typeof(UClass))]
    public class ClassPropertyDrawer : PropertyDrawer
    {
        private TArray<Type> _allTypes = default;
        private string[] _allNames = default;
        private int _selectedIndex = default;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_allTypes == null || _allTypes.Num() == 0)
            {
                FieldInfo info = property.GetFieldInfo(out object targetObject);
                TSubclassOfAttribute attribute = info.GetCustomAttribute<TSubclassOfAttribute>();
                AssemblyHelper.FindAll(attribute?.type ?? typeof(UObject), out _allTypes, attribute?.includingSelf ?? false);

                UClass value = (UClass)info.GetValue(targetObject);
                _selectedIndex = _allTypes.IndexOf(value);
                if (_selectedIndex == -1) _selectedIndex = default;

                _allNames = Array.ConvertAll(_allTypes.ToArray(), each => each.FullName);
            }

            EditorGUI.BeginChangeCheck();
            _selectedIndex = EditorGUI.Popup(position, label.text, _selectedIndex, _allNames);
            if (EditorGUI.EndChangeCheck())
            {
                property.SetValue((UClass)_allTypes[_selectedIndex]);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}
