using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class SetDestinationToNearestEdge : Behaviour
{
	public override Result Execute(EnemyData agent)
	{
		agent.Pathfinder.SetRoughDestination(agent.Target);
		return CONTINUE;
	}
}
