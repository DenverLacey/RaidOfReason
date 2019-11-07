/*
 * Author: Denver
 * Description:	Attack behaviour for the Suicide Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Suicide Enemy's attack behaviour
/// </summary>
public class SuicideEnemyAttack : Behaviour
{
	/// <summary>
	/// Performs attack behaviour on given Suicide Enemey agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If attack was successfully executed
	/// </returns>
	public override Result Execute(EnemyData agent) 
	{
		if (!agent.Attacking)
		{
			GameObject bombRadiusEffect = GameObject.Instantiate(agent.AttackPrefabs[1]);
			bombRadiusEffect.transform.position = new Vector3(agent.transform.position.x, 0.001f, agent.transform.position.z);
			bombRadiusEffect.transform.parent = agent.transform;
			ParticleSystem system = bombRadiusEffect.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = system.main;
			main.startSize = new ParticleSystem.MinMaxCurve(agent.AttackRange.max * bombRadiusEffect.transform.lossyScale.magnitude);
			GameObject.Destroy(bombRadiusEffect, agent.AttackCooldown);
		}

		agent.Attacking = true;

		agent.Pathfinder.StopPathing();

		agent.AttackTimer += Time.fixedDeltaTime;

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			foreach (BaseCharacter p in GameManager.Instance.AlivePlayers)
			{
				float sqrDistance = (p.transform.position - agent.transform.position).sqrMagnitude;
				if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max)
				{
					p.TakeDamage(agent.AttackDamage);
				}
			}

			var explosion = GameObject.Instantiate(agent.AttackPrefabs[0]);
			explosion.transform.position = agent.transform.position;
			agent.Die();

			return SUCCESS;
		}

		return PENDING_MONO;
	}
}
