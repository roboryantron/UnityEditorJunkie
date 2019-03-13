// ----------------------------------------------------------------------------
// Copyright © 2018 Schell Games, LLC. All Rights Reserved. 
// 
// Author: Ryan Hipple
// Date:   03/12/2019
// ----------------------------------------------------------------------------

using System;
using System.Reflection;
using UnityEngine;

namespace RoboRyanTron.QuickButtons
{
    
    // TODO: make a similar component for StringDisplay
    // calls a function / delegate that returns a string on its source object
    // drawer displays a "readonly" text field
    // This can be used to easily display things like properties or more complex
    // data in the inspector for debugging
    
    /// <summary>
    /// QuickButtons can be placed in any MonoBehaviour, ScriptableObject,
    /// StateMachineBehaviour or Serializable type to draw a button in the
    /// inspector with a custom callback. 
    /// </summary>
    [Serializable]
    public class QuickButton
    {
        #region -- Constants ---------------------------------------------------
        /// <summary>
        /// Broad set of binding flags used to reflect methods.
        /// </summary>
        private const BindingFlags flags = BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance;
        #endregion -- Constants ------------------------------------------------
        
        #region -- Private Variables -------------------------------------------
        /// <summary>
        /// Name of the function to invoke when this button is clicked.
        /// </summary>
        private readonly string functionName;
        
        /// <summary>
        /// Arguments to pass to the function named <see cref="functionName"/>
        /// when it is invoked.
        /// </summary>
        private readonly object[] functionArgs;
        
        /// <summary>
        /// Action to invoke when the button is pressed if this was constructed
        /// with the Action constructor. The object passed will be the object
        /// that this QuickButton is contained in. 
        /// </summary>
        private readonly Action<object> action;
        
        /// <summary>
        /// The call used to respond to a button press. This will be different
        /// based on what constructor was used.
        /// </summary>
        private readonly Action<object> invocation;
        #endregion -- Private Variables ----------------------------------------
        
        #region -- Constructors ------------------------------------------------
        public QuickButton(string functionName, params object[] functionArgs)
        {
            this.functionName = functionName;
            this.functionArgs = functionArgs;
            
            invocation = NameInvoke;
        }

        public QuickButton(Action<object> action)
        {
            this.action = action;
            
            invocation = ActionInvoke;
        }
        #endregion -- Constructors ---------------------------------------------
        
        #region -- Invocation --------------------------------------------------
        public void Invoke(object target)
        {
            invocation.Invoke(target);
        }
        
        private void ActionInvoke(object target)
        {
            action.Invoke(target);
        }
        
        private void NameInvoke(object target)
        {
            Type t = target.GetType();
            MethodInfo method = t.GetMethod(functionName, flags);
            if (method != null)
            {
                // TODO: error handling for argument length and types. This could handle a target invocation exception.
                method.Invoke(target, functionArgs);
            }
            else
            {
                Debug.LogError($"Unable to resolve method {functionName} from type {t.Name}");
            }
        }
        #endregion -- Invocation -----------------------------------------------
    }
}