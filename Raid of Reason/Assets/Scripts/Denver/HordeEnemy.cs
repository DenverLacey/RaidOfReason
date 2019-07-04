using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///	<summary>
///	Author:	Denver
///	Desc:	HordeEnemy class which derives from BaseEnemy.  HordeEnemy will spawn hordes of enemies
///			from a distance to the player and will not have any attacks of its own.
/// </summary>

/// <summary>
/// Enemy that will spawn hordes of other enemies from a distance.
/// </summary>
public class HordeEnemy : BaseEnemy
{
	[Tooltip("Maximum distance at which the enemy will stand and spawn horde.")]
	[SerializeField] private float m_maxStandingRange;

	[Tooltip("Minimum distance at which the enemy will stand and spawn horde.")]
	[SerializeField] private float m_minStandingDistance;

	[Tooltip("How quickly hordes will be spawned.")]
	[SerializeField] private float m_spawnSpeed = 3.0f;

	[Tooltip("How far away the horde will spawn away from the enemy.")]
	[SerializeField] private float m_spawningDistance = 1.0f;

	[Tooltip("How many enemies spawn per horde.")]
	[SerializeField] private int m_hordeSize = 2;

	[Tooltip("Prefabs for all the enemy types.")]
	[SerializeField] private GameObject[] m_enemyPrefabs;

	private float m_timer;

	protected override void Start() {
		base.Start();
		m_timer = 0f;
	}

	protected override void Taunted() {
		m_target = m_nashorn.transform.position;
		Attack();
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
				SpawnHorde();
				m_timer = m_spawnSpeed;
			}
		}
	}

	/// <summary>
	/// Spawns horde around enemy
	/// </summary>
	void SpawnHorde() {
		// calculate angle between each enemy in horde
		float spawnAngle = 360f / m_hordeSize;

		// spawn horde
		for (int i = 0; i < m_hordeSize; i++) {
			SpawnEnemy(spawnAngle * i);
		}
	}

	/// <summary>
	/// Spawns an horde enemy around this enemy
	/// </summary>
	/// <param name="angle">
	/// the angle between this enemy's forward and a vector from this enemy's position to the
	/// enemy's spawn position.
	/// </param>
	void SpawnEnemy(float angle) {
		// pick random enemy prefab
		int randIdx = Random.Range(0, m_enemyPrefabs.Length);

		// calculate enemy's spawn position
		Vector3 spawnVector = transform.right;
		spawnVector = Quaternion.AngleAxis(angle, Vector3.up) * spawnVector;
		spawnVector *= m_spawningDistance;

		Instantiate(m_enemyPrefabs[randIdx], spawnVector + transform.position, Quaternion.identity);
	}
}
