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
            // propogate success if child succeeds
            if (child.Execute(agent) == SUCCESS) 
            {
                return SUCCESS;
            }
        }
        // propagate failure if all children fails
        return FAILURE;
    }
}
