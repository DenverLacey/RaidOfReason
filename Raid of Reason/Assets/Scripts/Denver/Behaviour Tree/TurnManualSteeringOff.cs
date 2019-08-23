/*
 * Author: Denver
 * Description: TurnManualSteeringOff behaviour class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Set's a given EnemyData object's ManualSteering property to false
/// </summary>
public class TurnManualSteeringOff : Behaviour
{
	/// <summary>
	/// Turns off manual steering for a given agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If enemy isn't using manual steering
	/// </returns>
    public override Result Execute(EnemyData agent)
	{
		agent.ManualSteering = false;
		return SUCCESS;
	}
}
