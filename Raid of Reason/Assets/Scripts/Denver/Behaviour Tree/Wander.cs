using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Behaviour.Result;

/*
 * Author: Denver Lacey
 * Description:	Wander class for WanderBehaviour of the Behaviour Tree
 */

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
	public override Result Execute(EnemyData agent) {
		// don't find new position if already wandering
		if ((agent.NavMeshAgent.destination - agent.transform.position).sqrMagnitude <=  .2f) {
			return SUCCESS;
		}

		// calculate direction from current position to new wander destination
		float angle = Random.Range(0, 360);
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
		Vector3 direction = rotation.eulerAngles;

		Vector3 point = agent.transform.position + (direction * 5f);
		point = agent.FindClosestPoint(point);

		// return failure if a valid point couldn't be found
		if (point == Vector3.positiveInfinity) {
			return FAILURE;
		}

		// set agent's target and destination
		agent.Target = point;
		agent.NavMeshAgent.destination = agent.Target;

		return SUCCESS;
	}
}
