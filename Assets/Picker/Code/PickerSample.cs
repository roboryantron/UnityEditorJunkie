using UnityEngine;

namespace RoboRyanTron.Picker
{
    public class PickerSample : MonoBehaviour
    {
        [Picker]
        public ScriptableObject Object;
        
        public ScriptableObject OtherObject;
    }
}