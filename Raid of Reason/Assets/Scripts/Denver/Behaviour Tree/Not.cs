using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public override Behaviour.Result Execute(BaseEnemy agent) {
        switch (m_behaviour.Execute(agent)) {
            case Behaviour.Result.FAILURE:
                return Behaviour.Result.SUCCESS;

            case Behaviour.Result.SUCCESS:
                return Behaviour.Result.FAILURE;

            default:
                return Behaviour.Result.RUNNING;
        }
    }
}
