/*
 * Author: Denver
 * Description: MinAttackRangeCondition class that derives from Behaviour and tests distance
 *				between agent and its target and returns SUCCESS if distance is equal or less
 *				than agent's minimum attack range
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Behaviour for determining if an enemy's target is with min attack range
/// </summary>
public class MinAttackRangeCondition : Behaviour
{
	/// <summary>
	/// Returns SUCCESS if agent's target is within min attack range. FAILURE if otherwise
	/// </summary>
	/// <param name="agent">
	/// The agent to perform the behaviour on
	/// </param>
	/// <returns>
	/// If target is within min attack range
	/// </returns>
	public override Result Execute(EnemyData agent)
	{
		float sqrDistance = (agent.Target - agent.transform.position).sqrMagnitude;
		if (sqrDistance <= agent.AttackRange.min * agent.AttackRange.min)
		{
			return SUCCESS;
		}
		else
		{
			return FAILURE;
		}
	}
}
