/*
 * Author: Denver
 * Description: Fallback Behaviour class which makes an enemy avoid a player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Makes an enemy avoid a player
/// </summary>
public class Fallback : Behaviour
{
    /// <summary>
	/// Performs fallback behaviour on given enemy agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If path away from player was found
	/// </returns>
    public override Result Execute(EnemyData agent)
    {
		// calculate destination away from player
		// calculate destination
		Vector3 direction = (agent.transform.position - agent.Target).normalized;
		float avgAttackRange = (agent.AttackRange.max + agent.AttackRange.min) / 2f;
		Vector3 destination = agent.Target + direction * avgAttackRange;

		// set destination
		agent.SetDestination(destination);

		return SUCCESS;
    }
}
