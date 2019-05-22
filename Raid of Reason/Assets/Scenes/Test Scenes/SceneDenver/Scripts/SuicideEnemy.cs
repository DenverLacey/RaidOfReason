using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideEnemy : BaseEnemy
{
    [Tooltip("How close enemy must be before exploding.")]
    [SerializeField] private float m_attackRange;

    [Tooltip("Radius of the explosion.")]
    [SerializeField] private float m_damageRange;

    [Tooltip("How long until enemy explodes.")]
    [SerializeField] private float m_explosionCountdown;

    [Tooltip("Explosion Particle Effect.")]
    [SerializeField] private GameObject m_explosionPrefab;


    private bool m_readyToExplode = false;
    private float m_timer;
    

    protected override void Start() {
        base.Start();
        m_timer = m_explosionCountdown;
    }

    protected override void Update() {
        // determine state
		m_currentState = DetermineState();

		switch (m_currentState) {
			case AI_STATE.WANDER:
				Wander();
				break;

			case AI_STATE.ATTACK:
				Attack();
				break;

			default:
				Debug.LogError("State couldn't be determined!", this);
				break;
		}
        if (!m_readyToExplode) m_navMeshAgent.destination = m_target;
    }

    protected override AI_STATE DetermineState() {
        if (m_readyToExplode) return AI_STATE.ATTACK;

        AI_STATE s = AI_STATE.WANDER;

        float D = m_viewRange;

        foreach (var p in m_players) {
            if (!p) continue;

            float d = 0;
            if ((d = Vector3.Distance(transform.position, p.transform.position)) < D) {
                m_target = p.transform.position;
                D = d;
                s = AI_STATE.ATTACK;
            }
        }

        return s;
    }

    protected override void Attack() {
    	if (Vector3.Distance(transform.position, m_target) > 2f && m_oldState == AI_STATE.ATTACK) {
		return;
	}
	
        if (Vector3.Distance(transform.position, m_target) <= m_attackRange || m_readyToExplode) {
            m_readyToExplode = true;

            m_navMeshAgent.destination = transform.position;

            m_timer -= Time.deltaTime;

            if (m_timer <= 0.0f) {
                foreach (var p in m_players) {
                    if (Vector3.Distance(transform.position, p.transform.position) <= m_damageRange) {
                        p.TakeDamage(m_damage);
                    }
                }

                Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
