// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   03/12/2019
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace RoboRyanTron.QuickButtons
{
    [Serializable]
    public class QuickButton
    {
        [NonSerialized] public string function;
        [NonSerialized] public object[] args;

        public Action<object> action;
        
        public QuickButton(string functionName, params object[] args)
        {
            function = functionName;
            this.args = args;
        }

        public QuickButton(Action<object> action)
        {
            this.action = action;
        }
    }
}