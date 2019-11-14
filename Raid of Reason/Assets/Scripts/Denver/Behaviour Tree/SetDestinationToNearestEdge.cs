/*
 * Auhthor:	Denver
 * Description:	Behaviour used to set an enemy's position to the nearest viable position on the NavMesh in the behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Behaviour used to set enemy's position to the nearest viable position on the NavMesh. Does not effect the logic.
/// </summary>
public class SetDestinationToNearestEdge : Behaviour
{
	/// <summary>
	/// Sets agent's destination to closest viable position on the NavMesh to its target
	/// </summary>
	/// <param name="agent"> agent to perform behaviour on </param>
	/// <returns> Continue. Does not effect the logic of the tree </returns>
	public override Result Execute(EnemyData agent)
	{
		agent.Pathfinder.SetRoughDestination(agent.Target);
		return CONTINUE;
	}
}
