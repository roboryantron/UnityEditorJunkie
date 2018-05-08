// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   05/07/2018
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoboRyanTron.SceneReference
{
    [Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        public UnityEditor.SceneAsset Scene;
#endif

        [Tooltip("The name of the referenced scene. THis may be used at runtime to load the scene.")]
        public string SceneName;

        public int SceneIndex = -1;

        // TODO: IsAvailable - if it is not null, included in the build and enabled
        
        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            // TODO: exception loading the scene if it was not in the build settings
            SceneManager.LoadScene(SceneName, mode);
        }
        
        public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(SceneName, mode);
        }
        
        public void OnBeforeSerialize()
        {
            return;
            if (Scene != null)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(Scene);
                string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
                
                UnityEditor.EditorBuildSettingsScene[] scenes = 
                    UnityEditor.EditorBuildSettings.scenes;

                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == guid)
                    {
                        if(SceneIndex != i)
                            SceneIndex = i;
                        if (scenes[i].enabled)
                        {
                            if (SceneName != Scene.name)
                                SceneName = Scene.name;
                            return;
                        }
                        break;
                    }
                }

                if (SceneIndex >= 0)
                {
                    bool add = UnityEditor.EditorUtility.DisplayDialog("Scene Not In Build",
                        "You are refencing a scene that is not active the build. Enable it in the build settings now?",
                        "Yes", "No");
                    if (add)
                    {
                        UnityEditor.EditorBuildSettingsScene[] newScenes =
                            new UnityEditor.EditorBuildSettingsScene[scenes.Length];
                        Array.Copy(scenes, newScenes, scenes.Length);

                        newScenes[SceneIndex].enabled= true;
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