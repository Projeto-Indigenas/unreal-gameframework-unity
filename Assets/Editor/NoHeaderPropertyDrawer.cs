using UnityEditor;
using UnityEngine;
using UnrealEngine.Utilities;

namespace UnrealEditor.Utilities
{
    [CustomPropertyDrawer(typeof(NoHeaderAttribute))]
    public class NoHeaderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
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
