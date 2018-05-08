// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   05/07/2018
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace RoboRyanTron.SceneReference
{
    [Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        public UnityEditor.SceneAsset Scene;
#endif

        public string SceneName;
        
        public void OnBeforeSerialize()
        {
            if (Scene != null)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(Scene);
                string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
                
                UnityEditor.EditorBuildSettingsScene[] scenes = 
                    UnityEditor.EditorBuildSettings.scenes;

                int sceneIndex = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == guid)
                    {
                        sceneIndex = i;
                        if (scenes[i].enabled)
                        {
                            SceneName = Scene.name;
                            return;
                        }
                        break;
                    }
                }

                if (sceneIndex >= 0)
                {
                    bool add = UnityEditor.EditorUtility.DisplayDialog("Scene Not In Build",
                        "You are refencing a scene that is not active the build. Enable it in the build settings now?",
                        "Yes", "No");
                    if (add)
                    {
                        UnityEditor.EditorBuildSettingsScene[] newScenes =
                            new UnityEditor.EditorBuildSettingsScene[scenes.Length];
                        Array.Copy(scenes, newScenes, scenes.Length);

                        newScenes[sceneIndex].enabled= true;
                        UnityEditor.EditorBuildSettings.scenes = newScenes;
                    }
                }
                else
                {
                    // If the scene is not in the build settings.
                    bool add = UnityEditor.EditorUtility.DisplayDialog("Scene Not In Build",
                        "You are refencing a scene that is not added to the build. Add it to the editor build settings now?",
                        "Yes", "No");
                    if (add)
                    {
                        UnityEditor.EditorBuildSettingsScene[] newScenes =
                            new UnityEditor.EditorBuildSettingsScene[scenes.Length + 1];
                        Array.Copy(scenes, newScenes, scenes.Length);

                        newScenes[scenes.Length] = new UnityEditor.EditorBuildSettingsScene(
                            path, true);

                        UnityEditor.EditorBuildSettings.scenes = newScenes;
                    }
                }
            }
            else
            {
                SceneName = "";
            }
        }

        public void OnAfterDeserialize() {}
    }
}