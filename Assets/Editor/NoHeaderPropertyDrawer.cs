using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnrealEditor.Extensions;
using UnrealEngine.Utilities;

namespace UnrealEditor.Utilities
{
    [CustomPropertyDrawer(typeof(NoHeaderAttribute))]
    public class NoHeaderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0F;
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = property.Copy();
            endProperty.NextVisible(false);
            iterator.NextVisible(true);
            do
            {
                HeaderAttribute header = iterator.GetFieldInfo(out _).GetCustomAttribute<HeaderAttribute>();
                if (header != null) height += 20F;
                height += EditorGUI.GetPropertyHeight(iterator, label, true);
            }
            while (iterator.NextVisible(false) && !SerializedProperty.EqualContents(iterator, endProperty));
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = property.Copy();
            endProperty.NextVisible(false);
            iterator.NextVisible(true);
            do
            {
                position.height = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(position, iterator, true);
                position.y += position.height + 2F;
            }
            while (iterator.NextVisible(false) && !SerializedProperty.EqualContents(iterator, endProperty));
        }
    }
}
