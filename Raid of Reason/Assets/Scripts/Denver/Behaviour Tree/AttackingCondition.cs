using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingCondition : Behaviour
{
    public override Result Execute(EnemyData agent) {
		if (agent.Attacking) {
			return Result.SUCCESS;
		}
		else {
			return Result.FAILURE;
		}
	}
}
