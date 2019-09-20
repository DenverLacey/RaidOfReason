/*
 * Author: Denver
 * Description: Behaviour Tree Scriptable Object for Spawner Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;


public class SpawnerEnemyAttack : Behaviour
{
	/// <summary>
	/// Performs attack behaviour on spawner enemy agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If attack was successfully exectuted
	/// </returns>
	public override Result Execute(EnemyData agent)
	{
		// rotate to face player
		Vector3 direction = (agent.Target - agent.transform.position).normalized;
		direction.y = 0f;
		Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, .25f);

		// attack player
		agent.Attacking = true;
		agent.AttackTimer += Time.fixedDeltaTime;

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			// reset attacking variables
			agent.AttackTimer = 0f;

			// spawn attack prefab
			GameObject attackPrefab = ObjectPooling.GetPooledObject("EnemyDeathParticle");
			attackPrefab.transform.position = agent.transform.position;
			attackPrefab.SetActive(true);
			attackPrefab.GetComponent<ParticleSystem>().Play();

			// spawn child enemy
			GameObject child = Object.Instantiate(agent.AttackPrefabs[Random.Range(0, agent.AttackPrefabs.Length - 1)], agent.transform.position, Quaternion.identity);

			if (agent.Taunted)
			{
				EnemyData childData = child.GetComponentInChildren<EnemyData>();
				childData.Taunted = true;
			}
		}
		return PENDING_COMPOSITE;
	}
}
