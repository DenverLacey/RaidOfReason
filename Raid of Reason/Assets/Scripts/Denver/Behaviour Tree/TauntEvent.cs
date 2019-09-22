/*
 * Author: Denver
 * Description: TauntEvent behaviour that checks if Kreiger is taunting
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// A Behaviour that checks if Kreiger is taunting
/// </summary>
public class TauntEvent : Behaviour
{
	/// <summary>
    /// Checks if Kreiger is taunting
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If Kreiger is taunting
    /// </returns>
    public override Result Execute(EnemyData agent) 
	{
		if (GameManager.Instance.Kreiger && GameManager.Instance.Kreiger.isTaunting && agent.Taunted)
		{
			agent.Target = GameManager.Instance.Kreiger.transform.position;
			agent.TargetPlayer = GameManager.Instance.Kreiger;
			return SUCCESS;
		}
		else
		{
			agent.Taunted = false;
			return FAILURE;
		}
	}
}
