using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim, Denver Lacey
 * Description: A Base Class for All Objectives in the form of a Scriptable Object
 * Last Edited: 15/11/2019
 */
public abstract class BaseObjective : ScriptableObject
{
    // Initialisation for objectives that need it 
    public abstract void Init();
    // Update per Frame for objectives that need it
    public abstract void Update();
    // Returns a bool that is enabled on completion
    public abstract bool Completed();
    // Returns a bool that is enabled on failure
    public abstract bool Failed();
    // A timer for any time based objectives
    public abstract float Timer();
    // Returns a string that holds the description of the objective 
    public abstract string GrabDescription();
}
