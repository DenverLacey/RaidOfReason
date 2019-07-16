using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightlineCondition : Behaviour
{
    /// <sumarry>
    /// Checks if agent can see a player
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// A Behaviour.Result. If agent has a sightline with a player
    /// </returns>
    public override Behaviour.Result Execute(BaseEnemy agent) {
        float closestDist = float.MaxValue;
        Vector3 closestTar = Vector3.zero;

        foreach (BaseCharacter player in agent.Players) {
            Vector3 dir = player.transform.position - agent.transform.position;
            if (Physics.Raycast(agent.transform.position, dir, out RaycastHit info, closestDist)) {
                if (info.collider.tag == "Enemy") {
                    closestDist = info.distance;
                    closestTar = player.transform.position;
                }
            }
        }

        if (closestDist <= agent.ViewRange * agent.ViewRange) {
            agent.Target = closestTar;
            return Behaviour.Result.SUCCESS;
        }
        else {
            return Behaviour.Result.FAILURE;
        }
    }
}
