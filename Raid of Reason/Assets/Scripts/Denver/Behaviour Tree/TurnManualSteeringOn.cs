using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class TurnManualSteeringOn : Behaviour
{
    public override Result Execute(EnemyData agent)
	{
		agent.ManualSteering = true;
		return SUCCESS;
	}
}
