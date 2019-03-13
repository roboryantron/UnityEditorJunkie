// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   03/12/2019
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.QuickButtons.Editor
{
    [CustomPropertyDrawer(typeof(QuickButton))]
    public class QuickButtonDrawer : PropertyDrawer
    {

        public static object GetObjectForProperty(SerializedProperty property, int pathOffset = 0, bool skipList = true)
        {
            const BindingFlags flags = BindingFlags.Instance | 
                BindingFlags.NonPublic | BindingFlags.Public;
            
            Type t = property.serializedObject.targetObject.GetType();
            object obj = property.serializedObject.targetObject;

            string path = property.propertyPath.Replace(".Array.data[", "[");
            
            string[] props = path.Split('.');
            int end = props.Length - 1 + pathOffset;
            
            for (int i = 0; i < props.Length + pathOffset; i++)
            {
                string[] nameAndIndex = props[i].Split('[', ']');
                int arrayIndex = nameAndIndex.Length <= 1 ? -1 :
                    int.Parse(nameAndIndex[1]);
                FieldInfo field = t.GetField(nameAndIndex[0], flags);
                
                obj = field.GetValue(obj);
                t = field.FieldType;

                if (!skipList)
                {
                    if (i == end)
                        return obj;
                }
                
                if (arrayIndex >= 0)
                {
                    // If this is an arra
                    IList col = obj as IList;
                    if (col != null && arrayIndex < col.Count)
                    {
                        obj = col[arrayIndex];
                        t = t.IsArray ? t.GetElementType() : 
                            col.GetType().GetGenericArguments()[0];
                    }
                }
                
                if (i == end)
                    return obj;
            }
            return null;
        }
        
        public static object _GetObjectForProperty(SerializedProperty property, int pathOffset = 0)
        {
            Type t = property.serializedObject.targetObject.GetType();
            object obj = property.serializedObject.targetObject;
            string[] props = property.propertyPath.Split('.');
            int end = props.Length - 1 + pathOffset;
            for (int i = 0; i < props.Length + pathOffset; i++)
            {
                FieldInfo fi = t.GetField(props[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fi == null)
                {
                    if (props[i] == "Array")
                        continue;
                    if (props[i].Contains("["))
                    {
                        int index = int.Parse(props[i].Replace("data[", "").Replace("]", ""));
                        object[] obs = obj as object[];
                        if (obs != null)
                        {
                            if (index < obs.Length)
                            {
                                obj = obs[index];

                                if (i == end)
                                    return obj;

                                t = t.GetElementType();
                            }
                        }
                        else
                        {
                            IList col = obj as IList;
                            if (col != null && index < col.Count)
                            {
                                obj = col[index];

                                if (i == end)
                                    return obj;

                                t = col.GetType().GetGenericArguments()[0];
                            }
                        }
                    }
                }
                else
                {
                    obj = fi.GetValue(obj);

                    if (i == end)
                        return obj;

                    t = fi.FieldType;
                }
            }
            return null;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Magic number 15 is reflected from Unity's EditorGUI.cs
            // "internal static float indent" property
            
            position.xMin += EditorGUI.indentLevel * 15;

            if (GUI.Button(position, label))
            {
                QuickButton button = GetObjectForProperty(property) as QuickButton;
                if (button == null)
                {
                    Debug.LogError("Unable to resolve QuickButton from property " + property.propertyPath);
                    return;
                }
                
                object target = GetObjectForProperty(property, -1) ?? 
                    property.serializedObject.targetObject;

                button.Invoke(target);
            }
        }
    }
}