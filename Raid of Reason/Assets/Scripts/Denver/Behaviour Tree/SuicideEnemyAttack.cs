/*
 * Author: Denver
 * Description:	Attack behaviour for the Suicide Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		agent.Attacking = true;

		agent.Target = agent.transform.position;
		agent.NavMeshAgent.destination = agent.Target;

		agent.AttackTimer += Time.deltaTime;

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			foreach (BaseCharacter p in GameManager.Instance.Players)
			{
				if (!p) { continue; }
				float sqrDistance = (p.transform.position - agent.transform.position).sqrMagnitude;
				if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max)
				{
					p.TakeDamage(agent.AttackDamage);
                    if (p.tag == "Nashorn")
                        agent.isAttackingNashorn = true;                     
				}
			}

			GameObject.Instantiate(agent.AttackPrefabs[0], agent.transform.position, Quaternion.identity);
			agent.OnDeath();
		}

		return Result.SUCCESS;
	}
}
