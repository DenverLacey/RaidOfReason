/*
 * Author: Denver
 * Description: Advance behaviour class that makes an enemy move towards a player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Behaviour.Result;

/// <summary>
/// Makes an enemy move towards a player
/// </summary>
public class Advance : Behaviour
{
    /// <summary>
	/// Performs advance behaviour on given enemy agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If path to player was found
	/// </returns>
    public override Result Execute(EnemyData agent)
    {
        // calculate destination
        Vector3 direction = (agent.transform.position - agent.Target).normalized;

        float avgAttackRange = (agent.AttackRange.max + agent.AttackRange.min) / 2f;
        Vector3 destination = agent.Target + direction * avgAttackRange;

		// set destination
		agent.SetDestination(destination);
		
		return SUCCESS;
    }
}
