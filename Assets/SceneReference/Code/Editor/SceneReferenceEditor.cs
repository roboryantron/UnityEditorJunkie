using System;
using UnityEditor;
using UnityEngine;

namespace RoboRyanTron.SceneReference.Editor
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceEditor : PropertyDrawer
    {
        private const string ERROR_SCENE_MISSING =
            "You are refencing a scene that is not added to the build. Add it to the editor build settings now?";

        private const string ERROR_SCENE_DISABLED =
            "You are refencing a scene that is not active the build. Enable it in the build settings now?";

        private SerializedProperty scene;
        private SerializedProperty sceneName;
        private SerializedProperty sceneIndex;
        private SerializedProperty sceneEnabled;
        private SceneAsset sceneAsset;
        private string sceneAssetPath;
        private string sceneAssetGUID;

        private GUIContent errorIcon;
        private GUIStyle errorStyle;
        
        // TODO: draw warning icon if the scene is not in the build or not enabled

        private void CacheProperties(SerializedProperty property)
        {
            scene = property.FindPropertyRelative("Scene");
            sceneName = property.FindPropertyRelative("SceneName");
            sceneIndex = property.FindPropertyRelative("SceneIndex");
            sceneEnabled = property.FindPropertyRelative("SceneEnabled");
            sceneAsset = scene.objectReferenceValue as SceneAsset;

            if (sceneAsset != null)
            {
                sceneAssetPath = AssetDatabase.GetAssetPath(scene.objectReferenceValue);
                sceneAssetGUID = AssetDatabase.AssetPathToGUID(sceneAssetPath);
            }
            else
            {
                sceneAssetPath = null;
                sceneAssetGUID = null;
            }
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            CacheProperties(property);
            UpdateSceneState();

            position = ErrorCheck(position);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, scene, GUIContent.none, false);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                CacheProperties(property);
                UpdateSceneState();
                Validate();
            }
            
            EditorGUI.EndProperty();
        }

        private void UpdateSceneState()
        {
            if (sceneAsset != null)
            {
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

                sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == sceneAssetGUID)
                    {
                        if(sceneIndex.intValue != i)
                            sceneIndex.intValue = i;
                        sceneEnabled.boolValue = scenes[i].enabled;
                        if (scenes[i].enabled)
                        {
                            if (sceneName.stringValue != sceneAsset.name)
                                sceneName.stringValue = sceneAsset.name;
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

        private void DisplaySceneErrorPrompt(string message, bool insert)
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
                        sceneAssetPath, true);
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

        private Rect ErrorCheck(Rect position)
        {
            if (errorStyle == null)
            {
                errorStyle = "CN EntryErrorIconSmall";
                errorIcon = new GUIContent("", "error");
            }

            if (sceneAsset == null)
                return position;
            
            Rect warningRect = new Rect(position);
            warningRect.width = errorStyle.fixedWidth + 4;
            
            if (sceneIndex.intValue < 0)
            {
                errorIcon.tooltip = "Scene is not in build settings.";
                position.xMin = warningRect.xMax;   
                if (GUI.Button(warningRect, errorIcon, errorStyle))
                {
                    DisplaySceneErrorPrompt(ERROR_SCENE_MISSING, true);
                }
            }
            else if (!sceneEnabled.boolValue)
            {
                errorIcon.tooltip = "Scene is not enabled in build settings.";
                position.xMin = warningRect.xMax;
                if (GUI.Button(warningRect, errorIcon, errorStyle))
                {
                    DisplaySceneErrorPrompt(ERROR_SCENE_DISABLED, false);
                }
            }

            return position;
        }

        private void Validate()
        {
            if (sceneAsset != null)
            {
                EditorBuildSettingsScene[] scenes = 
                    EditorBuildSettings.scenes;

                sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == sceneAssetGUID)
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
                    DisplaySceneErrorPrompt(ERROR_SCENE_DISABLED, false);
                }
                else
                {
                    DisplaySceneErrorPrompt(ERROR_SCENE_MISSING, true);
                }
            }
            else
            {
                sceneName.stringValue = "";
            }
        }
    }
}