using System;
using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.SceneReference.Editor
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceEditor : PropertyDrawer
    {
        // TODO: draw warning icon if the scene is not in the build or not enabled
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            SerializedProperty scene = property.FindPropertyRelative("Scene");
            SerializedProperty sceneName = property.FindPropertyRelative("SceneName");
            SerializedProperty sceneIndex = property.FindPropertyRelative("SceneIndex");
            SceneAsset sceneAsset = scene.objectReferenceValue as SceneAsset;
            
            UpdateCache(scene, sceneName, sceneIndex);

            if (sceneAsset != null && sceneIndex.intValue < 0)
            {
                Rect warningRect = new Rect(position);
                
                GUIStyle errorStyle = "CN EntryErrorIconSmall";
                warningRect.width = errorStyle.fixedWidth + 4;
                position.xMin = warningRect.xMax;
                GUIContent content = new GUIContent("", "error");
                if (GUI.Button(warningRect, content, errorStyle))
                {
                    string path = AssetDatabase.GetAssetPath(scene.objectReferenceValue);
                    DisplaySceneErrorPrompt(ERROR_SCENE_MISSING, sceneIndex, path, true);
                }
                //GUI.Label(position, TYPE_ERROR);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, scene, GUIContent.none, false);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                Validate(property);
            }
            
            EditorGUI.EndProperty();
        }

        private void UpdateCache(SerializedProperty scene, SerializedProperty sceneName, SerializedProperty sceneIndex)
        {
            SceneAsset sceneAsset = scene.objectReferenceValue as SceneAsset;
            
            if (sceneAsset != null)
            {
                string path = AssetDatabase.GetAssetPath(scene.objectReferenceValue);
                string guid = AssetDatabase.AssetPathToGUID(path);
                
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

                sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == guid)
                    {
                        if(sceneIndex.intValue != i)
                            sceneIndex.intValue = i;
                        if (scenes[i].enabled)
                        {
                            if (sceneName.stringValue != sceneAsset.name)
                                sceneName.stringValue = sceneAsset.name;
                            return;
                        }
                        break;
                    }
                }
            }
            else
            {
                sceneName.stringValue = "";
            }
        }


        public void DisplaySceneErrorPrompt(string message, SerializedProperty sceneIndex, string path, bool insert)
        {
            EditorBuildSettingsScene[] scenes = 
                EditorBuildSettings.scenes;
            
            int add = EditorUtility.DisplayDialogComplex("Scene Not In Build",
                message,
                "Yes", "No", "Open Build Settings");
            if (add == 0)
            {
                int newCount = insert ? scenes.Length + 1 : scenes.Length;
                EditorBuildSettingsScene[] newScenes =
                    new EditorBuildSettingsScene[newCount];
                Array.Copy(scenes, newScenes, scenes.Length);

                if (insert)
                {
                    newScenes[scenes.Length] = new EditorBuildSettingsScene(
                        path, true);
                    sceneIndex.intValue = scenes.Length;
                }
                
                newScenes[sceneIndex.intValue].enabled= true;
                
                EditorBuildSettings.scenes = newScenes;
            }
            else if (add == 2)
            {
                EditorApplication.ExecuteMenuItem("File/Build Settings...");
            }
        }
        
        public class SceneErrorPrompt
        {
            
        }

        private const string ERROR_SCENE_MISSING =
            "You are refencing a scene that is not added to the build. Add it to the editor build settings now?";

        private const string ERROR_SCENE_DISABLED =
            "You are refencing a scene that is not active the build. Enable it in the build settings now?";
        
        public void Validate(SerializedProperty property)
        {
            SerializedProperty scene = property.FindPropertyRelative("Scene");
            SerializedProperty sceneName = property.FindPropertyRelative("SceneName");
            SerializedProperty sceneIndex = property.FindPropertyRelative("SceneIndex");
            
            SceneAsset sceneAsset = scene.objectReferenceValue as SceneAsset;
            
            if (sceneAsset != null)
            {
                
                string path = AssetDatabase.GetAssetPath(scene.objectReferenceValue);
                string guid = AssetDatabase.AssetPathToGUID(path);
                
                EditorBuildSettingsScene[] scenes = 
                    EditorBuildSettings.scenes;

                sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == guid)
                    {
                        if(sceneIndex.intValue != i)
                            sceneIndex.intValue = i;
                        if (scenes[i].enabled)
                        {
                            if (sceneName.stringValue != sceneAsset.name)
                                sceneName.stringValue = sceneAsset.name;
                            return;
                        }
                        break;
                    }
                }

                if (sceneIndex.intValue >= 0)
                {
                    DisplaySceneErrorPrompt(ERROR_SCENE_DISABLED, sceneIndex, path, false);
                }
                else
                {
                    DisplaySceneErrorPrompt(ERROR_SCENE_MISSING, sceneIndex, path, true);
                }
            }
            else
            {
                sceneName.stringValue = "";
            }
        }
    }
}