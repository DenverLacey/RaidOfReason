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
		Vector3 playerPosition = agent.Target;
		playerPosition.y = agent.transform.position.y;
		Vector3 dir = (playerPosition - agent.transform.position).normalized;
		Vector3 origin = agent.transform.position;
		origin.y = agent.transform.position.y;

		int ignoreEnemies = Utility.GetIgnoreMask("Enemy", "Ignore Raycast");
		if (Physics.Raycast(origin, dir, out RaycastHit info, agent.ViewRange, ignoreEnemies)) 
		{
			if (info.collider.tag == "Kenron" || info.collider.tag == "Nashorn" || info.collider.tag == "Thea")
			{
				return SUCCESS;
			}
		}
		return FAILURE;
    }
}