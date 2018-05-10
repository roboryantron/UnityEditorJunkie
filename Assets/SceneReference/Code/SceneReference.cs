// ---------------------------------------------------------------------------- 
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
        public class SceneLoadException : Exception
        {
            public SceneLoadException(string message) : base(message)
            {}
        }
        
#if UNITY_EDITOR
        public UnityEditor.SceneAsset Scene;
#endif

        [Tooltip("The name of the referenced scene. THis may be used at runtime to load the scene.")]
        public string SceneName;

        [SerializeField]
        private int sceneIndex = -1;

        [SerializeField]
        private bool sceneEnabled;

        private void ValidateScene()
        {
            if (string.IsNullOrEmpty(SceneName))
                throw new SceneLoadException("No scene specified.");
            
            if (sceneIndex < 0)
                throw new SceneLoadException("Scene " + SceneName + " is not in the build settings");
            
            if (!sceneEnabled)
                throw new SceneLoadException("Scene " + SceneName + " is not enabled in the build settings");
        }
        
        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            ValidateScene();
            SceneManager.LoadScene(SceneName, mode);
        }
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if (Scene != null)
            {
                string sceneAssetPath = UnityEditor.AssetDatabase.GetAssetPath(Scene);
                string sceneAssetGUID = UnityEditor.AssetDatabase.AssetPathToGUID(sceneAssetPath);
                
                UnityEditor.EditorBuildSettingsScene[] scenes = 
                    UnityEditor.EditorBuildSettings.scenes;

                sceneIndex = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == sceneAssetGUID)
                    {
                        sceneIndex = i;
                        sceneEnabled = scenes[i].enabled;
                        if (scenes[i].enabled)
                            SceneName = Scene.name;
                        break;
                    }
                }
            }
            else
            {
                SceneName = "";
            }
#endif
        }

        public void OnAfterDeserialize() {}
    }
}