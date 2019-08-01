﻿/*
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
		if (!GameManager.Instance.Nashorn)
		{
			return FAILURE;
		}
		else if (GameManager.Instance.Nashorn.isTaunting)
		{
			agent.Taunted = true;
			agent.Target = GameManager.Instance.Nashorn.transform.position;
			agent.NavMeshAgent.destination = agent.Target;
			return SUCCESS;
		}
		else
		{
			agent.Taunted = false;
			return FAILURE;
		}
	}
}
