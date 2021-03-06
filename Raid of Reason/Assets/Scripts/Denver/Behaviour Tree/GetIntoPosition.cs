﻿/*
 * Author: Denver
 * Description:	GetIntoPosition Behaviour class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Sets Enemy's destination to mid point of their attack range
/// </summary>
public class GetIntoPosition : Behaviour
{
	/// <summary>
	/// Sets agent's destination to be in position to attack at range
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
		agent.Pathfinder.SetDestination(destination);

		return SUCCESS;
	}
}
