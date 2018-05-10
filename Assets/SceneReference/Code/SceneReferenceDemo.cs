// ----------------------------------------------------------------------------
// Author: Ryan Hipple
// Date:   05/07/2018
// ----------------------------------------------------------------------------

using UnityEngine;

namespace RoboRyanTron.SceneReference
{
    public class SceneReferenceDemo : MonoBehaviour
    {
        public SceneReference SceneReferenceA;
        public SceneReference SceneReferenceB;

        public void LoadSceneA()
        {
            SceneReferenceA.LoadScene();
        }
        
        public void LoadSceneB()
        {
            SceneReferenceB.LoadScene();
        }
    }
}