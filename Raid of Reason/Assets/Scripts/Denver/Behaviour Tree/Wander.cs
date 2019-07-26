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
		if ((agent.NavMeshAgent.destination - agent.transform.position).sqrMagnitude >=  .3f) 
		{
			agent.NavMeshAgent.destination = agent.Target;
			return SUCCESS;
		}

		NavMeshTriangulation tris = NavMesh.CalculateTriangulation();
	
		// calculate indices of each vertex of a random triangle
		int rawIndex = 3 * Random.Range(0, Mathf.FloorToInt(tris.indices.Length / 3));
		int index1 = tris.indices[rawIndex];
		int index2 = tris.indices[rawIndex + 1];
		int index3 = tris.indices[rawIndex + 2];

		// calculate random offsets
		float offset1 = Random.Range(.25f, .75f);
		float offset2 = Random.Range(.25f, .75f);

		// move target point towards each vertex of the triangle by offsets
		Vector3 point = tris.vertices[index1];
		point = Vector3.Lerp(point, tris.vertices[index2], offset1);
		point = Vector3.Lerp(point, tris.vertices[index3], offset2);

		agent.Target = point;
		agent.NavMeshAgent.destination = agent.Target;

		return SUCCESS;
	}
}
