/*
 * Author: Denver
 * Description: Selector Composite Behaviour class for behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Composite Behaviour that acts like an 'or' expression for a behaviour tree
/// </summary>
public class Selector : Composite
{
    /// <summary>
    /// Runs children's execture functions and propagates result
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If one child was successful
    /// </returns>
    public override Result Execute(EnemyData agent) 
    {
        // run children's execute functions
        foreach (Behaviour child in m_children) 
        {
			Result result = child.Execute(agent);

			switch (result)
			{
				case SUCCESS:
					return SUCCESS;

				case PENDING_COMPOSITE:
					agent.PendingBehaviour = this;
					return PENDING_ABORT;

				case PENDING_MONO:
					agent.PendingBehaviour = child;
					return PENDING_ABORT;

				case PENDING_ABORT:
					return PENDING_ABORT;
			}
        }
        
		// propagate failure if all children fails
        return FAILURE;
    }
}
