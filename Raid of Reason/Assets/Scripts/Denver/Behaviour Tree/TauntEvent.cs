/*
 * Author: Denver
 * Description: TauntEvent behaviour that checks if Nashorn is taunting
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// A Behaviour that checks if Nashorn is taunting
/// </summary>
public class TauntEvent : Behaviour
{
	/// <summary>
    /// Checks if Nashorn is taunting
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If Nashorn is taunting
    /// </returns>
    public override Result Execute(EnemyData agent) 
	{
		if (GameManager.Instance.Nashorn && GameManager.Instance.Nashorn.isTaunting)
		{
			// determine square distance from agent and nashorn
			float sqrDistance = (agent.transform.position - GameManager.Instance.Nashorn.transform.position).sqrMagnitude;

			// if agent is within taunt radius
			if (sqrDistance <= GameManager.Instance.Nashorn.TauntRadius * GameManager.Instance.Nashorn.TauntRadius)
			{
				agent.Taunted = true;
				agent.Target = GameManager.Instance.Nashorn.transform.position;
				agent.TargetPlayer = GameManager.Instance.Nashorn;
				agent.TauntIcon.SetActive(true);
				return SUCCESS;
			}
		}
		else
		{
			agent.Taunted = false;
            agent.TauntIcon.SetActive(false);
		}
		return FAILURE;
	}
}
