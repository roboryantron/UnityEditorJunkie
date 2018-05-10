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

        // TODO: index and enabled could be put in a cache dictionary on the editor
        // -1 indicates that it is not in the scene list
        public int SceneIndex = -1;

        public bool SceneEnabled;

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
            if (Scene != null)
                SceneName = Scene.name;
        }

        public void OnAfterDeserialize() {}
    }
}