using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.Serialize.Editor
{
    [CustomPropertyDrawer(typeof(IInspectorValue))]
    public class SerializeFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty value = property.FindPropertyRelative("value");
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, value, label, false);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}