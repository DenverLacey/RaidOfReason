/*
 * Author: Denver
 * Description: Not behaviour class the returns the opposite result when executed
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Nots result given by behaviour
/// </summary>
public class Not : Behaviour
{
    private Behaviour m_behaviour;

    public Not(Behaviour behaviour) 
	{
        m_behaviour = behaviour;
    }

    /// <summary>
    /// Flips behaviour's result
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// The opposite of m_behaviour's result
    /// </returns>
    public override Result Execute(EnemyData agent) 
	{
        switch (m_behaviour.Execute(agent)) 
		{
            case FAILURE:
                return SUCCESS;

            case SUCCESS:
                return FAILURE;

            case CONTINUE:
                return CONTINUE;

			case PENDING_ABORT:
				return PENDING_ABORT;

			case PENDING_COMPOSITE:
				return PENDING_COMPOSITE;

			case PENDING_MONO:
				return PENDING_MONO;

			default:
				throw new System.InvalidOperationException("Unknown Behaviour result");
        }
    }
}
