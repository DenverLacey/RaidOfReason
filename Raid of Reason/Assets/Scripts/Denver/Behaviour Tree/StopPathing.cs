using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class StopPathing : Behaviour
{
	public override Result Execute(EnemyData agent)
	{
		agent.StopPathing();
		return SUCCESS;
	}
}
