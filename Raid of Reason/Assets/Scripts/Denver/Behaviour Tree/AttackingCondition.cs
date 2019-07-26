/*
 * Author: Denver
 * Description:	AttackingCondition behaviour that checks if an agent is currently attacking
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Checks if agent is attacking
/// </summary>
public class AttackingCondition : Behaviour
{
	/// <summary>
	/// Returns SUCCESS if agent is attacking and FAILURE if otherwise
	/// </summary>
	/// <param name="agent">
	/// The agent to perfom behaviour on
	/// </param>
	/// <returns>
	/// If agent is attacking
	/// </returns>
    public override Result Execute(EnemyData agent)
	{
		return agent.Attacking ? SUCCESS : FAILURE;
	}
}
