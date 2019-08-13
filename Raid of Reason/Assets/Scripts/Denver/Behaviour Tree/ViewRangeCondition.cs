/*
 * Author: Denver
 * Description:	ViewRangeCondition Behaviour for the behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result; 

/// <summary>
/// Finds closest player within enemy's view range
/// </summary>
public class ViewRangeCondition : Behaviour
{
    /// <sumarry>
    /// Determines if a player is within the agent's view range
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If a player is within the agent's view range
    /// </returns>
    public override Result Execute(EnemyData agent)
	{
        float closestDist = float.MaxValue;
        Vector3 closestTar = Vector3.zero;

        foreach (BaseCharacter player in GameManager.Instance.Players)
		{
			if (!player) { continue; }
			if (player.playerState != BaseCharacter.PlayerState.ALIVE) { continue; }

            float sqrDistance = (player.transform.position - agent.transform.position).sqrMagnitude;
            if (sqrDistance < closestDist)
			{
                closestDist = sqrDistance;
                closestTar = player.transform.position;
            }
        }

        if (closestDist <= agent.ViewRange * agent.ViewRange)
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
