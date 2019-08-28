/*
 * Author: Denver
 * Description:	SetDestination behaviour class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Sets enemy's path destination to the enemy's target
/// </summary>
public class SetDestination : Behaviour
{
	/// <summary>
	/// Sets enemy's NavMeshAgent destination to its target position
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If destination was set successfully
	/// </returns>
	public override Result Execute(EnemyData agent)
	{
		agent.Pathfinder.SetDestination(agent.Target);
		return SUCCESS;
	}
}
