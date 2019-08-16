/*
 * Author: Denver
 * Description:	Abstract Behaviour class that all behaviours will derive from and also
 *				encapsulates Result enum
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for all Behaviours
/// </summary>
public abstract class Behaviour
{
    public enum Result
    {
        FAILURE,
        SUCCESS,
        PENDING_COMPOSITE,
		PENDING_MONO,
		PENDING_ABORT
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
    public abstract Result Execute(EnemyData agent);

	public static implicit operator bool(Behaviour b)
	{
		return b != null;
	}
}
