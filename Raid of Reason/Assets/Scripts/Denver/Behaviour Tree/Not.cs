using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Behaviour.Result;

public class Not : Behaviour
{
    private Behaviour m_behaviour;

    Not(Behaviour behaviour) {
        m_behaviour = behaviour;
    }

    /// <summary>
    /// Flips behaviour's result
    /// </summary>
     /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// A Behaviour.Result. Behaviour's result notted
    /// </returns>
    public override Result Execute(EnemyData agent) {
        switch (m_behaviour.Execute(agent)) {
            case FAILURE:
                return SUCCESS;

            case SUCCESS:
                return FAILURE;

            default:
                return PENDING;
        }
    }
}
