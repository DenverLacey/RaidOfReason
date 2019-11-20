/*
 * Author: Denver
 * Description: StunnedCondition behaviour class that checks if an enemy is stunned
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that checks if an enemy is stunned
/// </summary>
public class StunnedCondition : Behaviour
{
	/// <summary>
    /// Checks if agent is stunned
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If agent is stunned
    /// </returns>
	public override Result Execute(EnemyData agent) 
	{
		if (agent.Stunned) 
		{
			return Result.SUCCESS;
		}
		else 
		{
			return Result.FAILURE;
		}
	}
}
