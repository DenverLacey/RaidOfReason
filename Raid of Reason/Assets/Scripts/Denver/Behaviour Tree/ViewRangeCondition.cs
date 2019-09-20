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
        float closestSqrDist = float.MaxValue;
        Vector3 closestTar = Vector3.zero;
		BaseCharacter closestCharacter = null;

        foreach (BaseCharacter player in GameManager.Instance.Players)
		{
			if (!Utility.PlayerIsAttackable(player))
				continue;

			float sqrDistance = (player.transform.position - agent.transform.position).sqrMagnitude;

			bool isPriority = false;
			if (player.CharacterType == agent.PriorityCharacter)
			{
				sqrDistance -= agent.SqrPriorityThreashold;
				isPriority = true;
			}

			bool shouldChange = false;
			if (isPriority && sqrDistance <= closestSqrDist ||
				sqrDistance < closestSqrDist)
			{
				shouldChange = true;
			}

			if (shouldChange)
			{
				closestSqrDist = sqrDistance;
				closestTar = player.transform.position;
				closestCharacter = player;
			}
        }

        if (closestSqrDist <= agent.SqrViewRange)
		{
            agent.Target = closestTar;
			agent.TargetPlayer = closestCharacter;
            return SUCCESS;
        }
        else
		{
            return FAILURE;
        }
    }
}
