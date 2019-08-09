using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class TauntCondition : Behaviour
{
	public override Result Execute(EnemyData agent)
	{
		if (agent.Taunted)
		{
			return SUCCESS;
		}
		else
		{
			return FAILURE;
		}
	}
}
