/*
 * Author: Denver
 * Description:	MaxAttackRangeCondition class that derives from Behaviour and tests distance
 *				between agent and its target and returns SUCCESS if distance is equal or less
 *				than agent's maximum attack range
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Behaviour for determining if an enemy's target is with max attack range
/// </summary>
public class MaxAttackRangeCondition : Behaviour
{
	/// <summary>
	/// Returns SUCCESS if agent's target is within max attack range. FAILURE if otherwise
	/// </summary>
	/// <param name="agent">
	/// The agent to perform the behaviour on
	/// </param>
	/// <returns>
	/// If target is within max attack range
	/// </returns>
	public override Result Execute(EnemyData agent)
	{
		float sqrDistance = (agent.Target - agent.transform.position).sqrMagnitude;
		if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max)
		{
			return SUCCESS;
		}
		else
		{
			return FAILURE;
		}
	}
}
