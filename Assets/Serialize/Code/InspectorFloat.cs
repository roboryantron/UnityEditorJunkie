using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RoboRyanTron.Serialize
{
    public interface IInspectorValue
    {
    }

    [Serializable]
    public struct InspectorFloat : IInspectorValue
    {
        [UsedImplicitly][SerializeField] private float value;

        [UsedImplicitly] public float Value { get { return value; } }
        
        public static implicit operator float(InspectorFloat f)
        { return f.value; }
    }
    
    [Serializable]
    public struct InspectorInt: IInspectorValue
    {
        [UsedImplicitly][SerializeField] private int value;

        [UsedImplicitly] public int Value { get { return value; } }
        
        public static implicit operator int(InspectorInt f)
        { return f.value; }
    }
    
    [Serializable]
    public struct InspectorBool : IInspectorValue
    {
        [UsedImplicitly][SerializeField] private bool value;

        [UsedImplicitly] public bool Value { get { return value; } }
        
        public static implicit operator bool(InspectorBool f)
        { return f.value; }
    }
}