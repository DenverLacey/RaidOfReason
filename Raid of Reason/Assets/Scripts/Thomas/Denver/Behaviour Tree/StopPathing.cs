/*
 * Author: Denver
 * Description:	StopPathing behaviour class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Calls StopPathing function on a given EnemyData object
/// </summary>
public class StopPathing : Behaviour
{
	/// <summary>
	/// Stops agent pathing
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If agent has stopped pathing
	/// </returns>
	public override Result Execute(EnemyData agent)
	{
		agent.Pathfinder.StopPathing();
		return SUCCESS;
	}
}
