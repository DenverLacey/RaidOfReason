using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Behaviour.Result;

public class Pathfinder : Behaviour
{
    public override Result Execute(EnemyData agent) {
        agent.NavMeshAgent.destination = agent.Target;

        if (agent.NavMeshAgent.hasPath) {
            return SUCCESS;
        }
        else {
            return FAILURE;
        }
    }
}
