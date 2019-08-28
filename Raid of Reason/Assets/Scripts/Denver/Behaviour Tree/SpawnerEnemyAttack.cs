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
			agent.Attacking = false;
			agent.AttackTimer = 0f;

			agent.Pathfinder.SetDestination(agent.transform.position);

			float angle = 360f / agent.AttackDamage;

			for (int i = 0; i < agent.AttackDamage; i++)
			{
				// calculate enemy's spawn position
				Vector3 spawnVector = agent.transform.right;
				spawnVector = Quaternion.AngleAxis(angle * i, Vector3.up) * spawnVector;

				// check that no obstacle where we want to spawn enemy
				if (!Physics.SphereCast(agent.transform.position, 1f, spawnVector, out RaycastHit info, 2f))
				{
					int randIdx = Random.Range(0, agent.AttackPrefabs.Length);
					spawnVector *= 2.5f;
					GameObject.Instantiate(agent.AttackPrefabs[randIdx], spawnVector + agent.transform.position, agent.transform.rotation);
				}
			}
		}
		return PENDING_COMPOSITE;
	}
}
