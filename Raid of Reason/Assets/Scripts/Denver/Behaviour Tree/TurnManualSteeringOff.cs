using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class TurnManualSteeringOff : Behaviour
{
    public override Result Execute(EnemyData agent)
	{
		agent.ManualSteering = false;
		return SUCCESS;
	}
}
