using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntEvent : Behaviour
{
    public override Result Execute(EnemyData agent) 
	{
		Nashorn nashorn = null;

		foreach (var p in agent.Players) 
		{
			nashorn = p as Nashorn;

			if (nashorn) break;
		}

		if (!nashorn) 
		{
			return Result.FAILURE;
		}

		if (nashorn.isActive) 
		{
			agent.Taunted = true;
			agent.Target = nashorn.transform.position;
			return Result.SUCCESS;
		}
		else 
		{
			agent.Taunted = false;
			return Result.FAILURE;
		}
	}
}
