using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedCondition : Behaviour
{
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
