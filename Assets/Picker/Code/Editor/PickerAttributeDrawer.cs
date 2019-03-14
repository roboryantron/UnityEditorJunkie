// ----------------------------------------------------------------------------
// Author: Ryan Hipple
// Date:   08/17/2018
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.Picker.Editor
{
    [CustomPropertyDrawer(typeof(PickerAttribute))]
    public class PickerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
        }
    }
}