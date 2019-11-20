/*
 * Author: Denver
 * Description: Sequence Composite Behaviour class for behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Composite Behaviour that acts like an 'and' expression for a behaviour tree
/// </summary>
public class Sequence : Composite
{
    /// <summary>
    /// Runs children's execture functions and propagates result
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// If children were all successful
    /// </returns>
    public override Result Execute(EnemyData agent) 
    {
        // run children's execute functions
        foreach (Behaviour child in m_children) 
        {
			Result result = child.Execute(agent);

			switch (result)
			{
				case FAILURE:
					return FAILURE;

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

        // propagate success if all children suceed
        return SUCCESS;
    }
}
