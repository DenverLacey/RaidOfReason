using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public override Behaviour.Result Execute(BaseEnemy agent) {
        // run children's execute functions
        foreach (Behaviour child in m_children) {
            // propogate success if child succeeds
            if (child.Execute(agent) == Behaviour.Result.SUCCESS) {
                return Behaviour.Result.SUCCESS;
            }
        }
        // propogate failure if all children fails
        return Behaviour.Result.SUCCESS;
    }
}
