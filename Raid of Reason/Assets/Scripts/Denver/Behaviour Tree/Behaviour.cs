using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviour
{
    public enum Result
    {
        FAILURE,
        SUCCESS,
        PENDING
    }

	/// <summary>
	/// Executes Behaviours functionality
	/// </summary>
	/// <param name="agent">
	/// Agent that behaviour's functionality should be acted upon
	/// </param>
	/// <returns>
	/// If behaviour was successful; if it failed or is pending
	/// </returns>
    abstract public Result Execute(EnemyData agent);
}
