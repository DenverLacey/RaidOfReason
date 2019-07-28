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
            // propogate failure if child fails
            if (child.Execute(agent) == FAILURE) 
            {
                return FAILURE;
            }
        }
        // propagate success if all children suceed
        return SUCCESS;
    }
}
