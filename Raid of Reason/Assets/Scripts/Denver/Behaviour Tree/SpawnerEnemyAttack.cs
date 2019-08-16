using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class SpawnerEnemyAttack : Behaviour
{
	public override Result Execute(EnemyData agent)
	{
		// rotate to face player
		Vector3 direction = (agent.Target - agent.transform.position).normalized;
		direction.y = agent.transform.position.y;
		Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, .25f);

		// attack player
		agent.Attacking = true;
		agent.AttackTimer += Time.deltaTime;

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			// reset attacking variables
			agent.Attacking = false;
			agent.AttackTimer = 0f;

			agent.SetDestination(agent.transform.position);

			float angle = 360f / agent.AttackDamage;

			for (int i = 0; i < agent.AttackDamage; i++)
			{
				int randIdx = Random.Range(0, agent.AttackPrefabs.Length);

				// calculate enemy's spawn position
				Vector3 spawnVector = agent.transform.right;
				spawnVector = Quaternion.AngleAxis(angle * i, Vector3.up) * spawnVector;
				spawnVector *= 2f;

				GameObject.Instantiate(agent.AttackPrefabs[randIdx], spawnVector + agent.transform.position, agent.transform.rotation);
			}
		}
		return SUCCESS;
	}
}
