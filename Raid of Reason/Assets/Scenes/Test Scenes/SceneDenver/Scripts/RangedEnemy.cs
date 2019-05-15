using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy {

	public float maxStandingRange;
	public float minStandingDistance;

	protected override void Attack() {
		if (m_oldState == AI_STATE.ATTACK && m_navMeshAgent.remainingDistance < .25f) {
			return;
		}

		float dist = Vector3.Distance(m_target, transform.position);

		if (dist > maxStandingRange) {
			Vector3 direction = (transform.position - m_target).normalized;
			Vector3 destination = m_target + direction * maxStandingRange;
			destination.y = transform.position.y;

			Vector3 closest = FindClosestPoint(destination);
			m_target = closest;
		}
		else if (dist < minStandingDistance) {
			Vector3 direction = (transform.position - m_target).normalized;
			Vector3 destination = m_target + direction * minStandingDistance;
			destination.y = transform.position.y;

			Vector3 closest = FindClosestPoint(destination);
			m_target = closest;
		}
		else {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_target - transform.position, Vector3.up), 0.1f);
			m_target = transform.position;
		}

		m_oldState = AI_STATE.ATTACK;
	}
}
