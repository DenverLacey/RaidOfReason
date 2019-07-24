using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCondition : Behaviour
{
    /// <sumarry>
    /// Checks if agent is within a certain proximity of the players
    /// </summary>
    /// <param name="agent">
    /// The agent to execute behaviour on
    /// </param>
    /// <returns>
    /// A Behaviour.Result. If agent is in proximity with a player
    /// </returns>
    public override Behaviour.Result Execute(EnemyData agent) {
        float closestDist = float.MaxValue;
        Vector3 closestTar = Vector3.zero;

        foreach (BaseCharacter player in agent.Players) {
            if (!player) continue;

            float sqrDistance = (player.transform.position - agent.transform.position).sqrMagnitude;
            if (sqrDistance < closestDist) {
                closestDist = sqrDistance;
                closestTar = player.transform.position;
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
