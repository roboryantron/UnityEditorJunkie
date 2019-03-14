using UnityEngine;

namespace RoboRyanTron.Serialize
{
    public class SerializeDemo : MonoBehaviour
    {
        public InspectorFloat Float;
        public float RealFloat;

        public InspectorBool Bool;

        public InspectorInt Int;

        private void OnValidate()
        {
            RealFloat = Float;
        }
    }
}