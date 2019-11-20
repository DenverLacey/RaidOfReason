/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour Tree scriptable object class
/// </summary>
public abstract class BehaviourTree : ScriptableObject
{
	/// <summary>
	/// Executes Behaviours functionality
	/// </summary>
	/// <param name="agent">
	/// Agent that behaviour's functionality should be acted upon
	/// </param>
	/// <returns>
	/// If behaviour was successful; if it failed or is pending
	/// </returns>
	public abstract void Execute(EnemyData agent);
}