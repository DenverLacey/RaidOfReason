/*
 * Author: Denver Lacey
 * Description:	Wander class for WanderBehaviour of the Behaviour Tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Behaviour.Result;

/// <summary>
/// Performs Wander behaviour on an EnemyData object
/// </summary>
public class Wander : Behaviour
{
	/// <summary>
	/// Performs Wandering Behaviour on an agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perfom behaviour on
	/// </param>
	/// <returns>
	/// If agent can wander
	/// </returns>
	public override Result Execute(EnemyData agent) 
	{
		// don't find new position if already wandering
		if (!agent.Pathfinder.AtPosition(agent.Target)) 
		{
			return SUCCESS;
		}

		Vector3 lookAt = agent.Target;
		agent.Target = agent.Zone.GetRandomPoint(agent.transform.position.y);
		agent.Pathfinder.SetDestination(agent.Target, lookAt);

		return SUCCESS;
	}
}
