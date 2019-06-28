using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [Tooltip("How far can enemy reach with melee attack.")]
    [SerializeField] private float m_attackRange;

    [Tooltip("How fast the enemy can make consecutive melees.")]
    [SerializeField] private float m_meleeCooldown;

    private float m_meleeTimer;
    private BaseCharacter m_targetPlayer;

    protected override void Start() {
        base.Start();
        m_meleeTimer = m_meleeCooldown;
    }

    protected override AI_STATE DetermineState() {
        AI_STATE s = AI_STATE.WANDER;

        Transform closest = null;
		float D = m_viewRange;

		foreach (var p in m_players) {
            if (!p) continue;

			Ray ray = new Ray(transform.position, p.transform.position - transform.position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, D)) {
				if (hit.collider.tag == "Kenron" || hit.collider.tag == "Thea" || hit.collider.tag == "Nashorn") {
					closest = p.transform;
                    m_targetPlayer = p;
					D = Vector3.Distance(transform.position, p.transform.position);
					s = AI_STATE.ATTACK;
				}
			}
		}

        if (closest) {
            m_target = closest.position;
        }

		return s;
    }

    protected override void Attack() { 
        if (Vector3.Distance(transform.position, m_target) <= m_attackRange) {
            m_target = transform.position;
            m_meleeTimer -= Time.deltaTime;
            if (m_meleeTimer <= 0.0f) {
                if (m_targetPlayer) {
                    m_targetPlayer.TakeDamage(m_damage);
                    m_meleeTimer = m_meleeCooldown;
                }
            }
        }
        else {
            m_meleeTimer = m_meleeCooldown;
        }
    }
}
