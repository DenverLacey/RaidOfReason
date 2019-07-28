/*
 * Author: Denver
 * Description: Pathfinder behaviour class that sets an agent's NavMeshAgent destination 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Pathfinder behaviour that sets an agent's NavMeshAgent destination
/// </summary>
public class Pathfinder : Behaviour
{
    /// <summary>
    /// Set's agent's NavMeshAgent destination
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If path was found
    /// </returns>
    public override Result Execute(EnemyData agent) 
    {
        agent.NavMeshAgent.destination = agent.Target;

        if (agent.NavMeshAgent.hasPath) 
        {
            return SUCCESS;
        }
        else 
        {
            return FAILURE;
        }
    }
}
