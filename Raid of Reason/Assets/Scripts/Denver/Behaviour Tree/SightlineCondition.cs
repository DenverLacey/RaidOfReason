/*
 * Author: Denver
 * Description: SlightlineCondition behaviour class that checks if an enemy can see 
 *              a player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Behaviour that checks if agent can see a player
/// </summary>
public class SightlineCondition : Behaviour
{
    /// <summary>
    /// Checks if agent can see a player
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If agent has a sightline with a player
    /// </returns>
    public override Result Execute(EnemyData agent) 
    {
        float closestDist = float.MaxValue;
        Vector3 closestTar = Vector3.zero;

        foreach (BaseCharacter player in agent.Players) 
        {
            if (!player) { continue; }

            Vector3 dir = (player.transform.position - agent.transform.position).normalized;
            if (Physics.Raycast(agent.transform.position, dir, out RaycastHit info, closestDist)) 
            {
                if (info.collider.tag == "Enemy") 
                {
                    closestDist = info.distance;
                    closestTar = player.transform.position;
                }
            }
        }

        if (closestDist <= agent.ViewRange) 
        {
            agent.Target = closestTar;
            return SUCCESS;
        }
        else 
        {
            return FAILURE;
        }
    }
}
