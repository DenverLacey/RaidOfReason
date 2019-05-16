using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideEnemy : BaseEnemy
{
    protected override AI_STATE DetermineState() {
        AI_STATE s = AI_STATE.WANDER;

        float D = m_viewRange;

        foreach (var p in m_players) {
            float d = 0;
            if ((d = Vector3.Distance(transform.position, p.transform.position)) < D) {
                m_target = p.transform.position;
                D = d;
                s = AI_STATE.ATTACK;
            }
        }

        return s;
    }

    protected override void Attack() { }
}
