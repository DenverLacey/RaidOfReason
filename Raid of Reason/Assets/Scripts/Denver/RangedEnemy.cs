using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author:	Denver
/// Desc:	RangedEnemy class that derives from BaseEnemy.  RangedEnemy will shoot players from a
///			distance.
/// </summary>

/// <summary>
/// Enemy that will shoot players from a distance.
/// </summary>
public class RangedEnemy : BaseEnemy {

	[Tooltip("Maximum distance at which the enemy will stand and shoot.")]
	[SerializeField] private float m_maxStandingRange;

	[Tooltip("Minimum distance at which the enemy will stand and shoot.")]
	[SerializeField] private float m_minStandingDistance;

	[Tooltip("Projectile Prefab.")]
	[SerializeField] GameObject m_projectilePrefab;

	[Tooltip("How long in between shots.")]
	[SerializeField] private float m_shootCooldown;

	private float m_timer;

	protected override void Start() {
		base.Start();
		m_timer = m_shootCooldown;
	}

	protected override void Attack() {
		float dist = Vector3.Distance(m_target, transform.position);

		if (dist > m_maxStandingRange) {
			Vector3 direction = (transform.position - m_target).normalized;
			Vector3 destination = m_target + direction * m_maxStandingRange;
			destination.y = transform.position.y;

			Vector3 closest = FindClosestPoint(destination);
			m_target = closest;
		}
		else if (dist < m_minStandingDistance) {
			Vector3 direction = (transform.position - m_target).normalized;
			Vector3 destination = m_target + direction * m_minStandingDistance;
			destination.y = transform.position.y;

			Vector3 closest = FindClosestPoint(destination);
			m_target = closest;
		}
		else {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_target - transform.position, Vector3.up), 0.1f);
			m_target = transform.position;

			m_timer -= Time.deltaTime;

			if (m_timer <= 0.0f) {
				EnemyProjectile ep = Instantiate(m_projectilePrefab, transform.position + transform.forward, transform.rotation).GetComponent<EnemyProjectile>();
				ep.SetDamage(m_damage);
				m_timer = m_shootCooldown;
			}
		}
	}

	protected override void Taunted() {
		m_target = m_nashorn.transform.position;
		Attack();
	}
}
