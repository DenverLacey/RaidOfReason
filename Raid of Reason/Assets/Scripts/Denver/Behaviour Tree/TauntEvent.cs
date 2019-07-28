/*
 * Author: Denver
 * Description: TauntEvent behaviour that checks if Nashorn is taunting
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// A Behaviour that checks if Nashorn is taunting
/// </summary>
public class TauntEvent : Behaviour
{
	/// <summary>
    /// Checks if Nashorn is taunting
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If Nashorn is taunting
    /// </returns>
    public override Result Execute(EnemyData agent) 
	{
		Nashorn nashorn = null;

		foreach (var p in agent.Players) 
		{
			nashorn = p as Nashorn;

			if (nashorn) { break; }
		}

		if (!nashorn) 
		{
			return FAILURE;
		}

		if (nashorn.isActive) 
		{
			agent.Taunted = true;
			agent.Target = nashorn.transform.position;
			return SUCCESS;
		}
		else 
		{
			agent.Taunted = false;
			return FAILURE;
		}
	}
}
