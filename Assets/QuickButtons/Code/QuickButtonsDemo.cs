// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   03/12/2019
// ----------------------------------------------------------------------------

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RoboRyanTron.QuickButtons
{
    public class QuickButtonsDemo : MonoBehaviour
    {
        [Serializable]
        public class Nested
        {
            public int y = 66;
            
            [SerializeField] private QuickButton NestedButton = 
                new QuickButton("NestedBloop", "message!");
            
            public QuickButton NestedDelegateButton = new QuickButton(input =>
            {
                Nested nested = input as Nested;
                Debug.Log("Test simple " + nested.y);
            });
            
            private void NestedBloop(string msg)
            {
                Debug.Log("more bloop? " + msg);
            }
        }

        private const QuickButtonsDemo template = null;
        
        public int x = 35;

        public Nested[] nested;
 
        public QuickButton SimpleNameButton = new QuickButton("Test");
        
        public QuickButton DelegateButton = new QuickButton(input =>
        {
            QuickButtonsDemo demo = input as QuickButtonsDemo;
            Debug.Log("Test simple " + demo.x);
        });
        
        public QuickButton DelegateButtonMinimal = new QuickButton(input =>
            Debug.Log("Test simple " + (input as QuickButtonsDemo).x));

#if UNITY_EDITOR
        [SerializeField] [UsedImplicitly]
        private QuickButton DecoratedButton =
            new QuickButton(nameof(template.Bloop), "derp");
#endif

        
        private void Test() => Debug.Log("Test");
        
        private static void Bloop(string msg)
        {
            Debug.Log("Did someone say bloop? " + msg);
        }
    }
}