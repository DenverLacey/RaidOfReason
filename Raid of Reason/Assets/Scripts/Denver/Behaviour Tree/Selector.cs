using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Behaviour.Result;

public class Selector : Composite
{
    /// <sumarry>
    /// Runs children's execture functions and propigates result
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// A Behaviour.Result. If children were successful or failed
    /// </returns>
    public override Behaviour.Result Execute(EnemyData agent) {
        // run children's execute functions
        foreach (Behaviour child in m_children) {
            // propogate success if child succeeds
            if (child.Execute(agent) == SUCCESS) {
                return SUCCESS;
            }
        }
        // propogate failure if all children fails
        return FAILURE;
    }
}
