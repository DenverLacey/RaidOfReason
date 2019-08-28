/*
 * Author: Denver
 * Description:	TurnManualSteeringOn behaviour class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Sets a given EnemyData object's ManualSteering property to true
/// </summary>
public class TurnManualSteeringOn : Behaviour
{
	/// <summary>
	/// Turns on manual steering for a given agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If agent isn't automatically steering
	/// </returns>
    public override Result Execute(EnemyData agent)
	{
		agent.Pathfinder.manualSteering = true;
		return SUCCESS;
	}
}
