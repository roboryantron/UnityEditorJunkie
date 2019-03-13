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
            private const Nested template = null;
            [SerializeField] private QuickButton DemoButton = 
                new QuickButton(nameof(template.NestedBloop), "doip");
            
            private void NestedBloop(string msg)
            {
                Debug.Log("more bloop? " + msg);
            }
        }

        public int x = 35;

        public Nested[] nested;
        
        private const QuickButtonsDemo template = null;   
 
        public QuickButton Button = new QuickButton("Test");
        
        //public QuickButton Button2 = new QuickButton(()=>Debug.Log("Test simple"));
        
        public QuickButton Button3 = new QuickButton(input =>
        {
            QuickButtonsDemo demo = input as QuickButtonsDemo;
            Debug.Log("Test simple " + demo.x);
        });

#if UNITY_EDITOR
        [SerializeField] [UsedImplicitly]
        private QuickButton DemoButton = 
            new QuickButton(nameof(template.Bloop), "derp");
#endif

        
        private void Test() => Debug.Log("Test");
        
        private static void Bloop(string msg)
        {
            Debug.Log("Did someone say bloop? " + msg);
        }
    }
}