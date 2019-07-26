/*
 * Author: Denver
 * Description:	Attack behaviour for the Melee Enemy Type
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Melee Enemy's attack behaviour
/// </summary>
public class MeleeEnemyAttack : Behaviour
{
	/// <summary>
	/// Performs attack behaviour on given melee enemy agent
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

		agent.AttackTimer += Time.deltaTime;

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			// reset attacking variables
			agent.Attacking = false;
			agent.AttackTimer = 0.0f;

			// attack player
			// TODO: Replace with Instantiate(agent.AttackPrefab, agent.transform.position + agent.transform.forward, Quaternion.identity);
			if (Physics.Raycast(agent.transform.position, agent.transform.forward, out RaycastHit hit, agent.AttackRange.max))
			{
				BaseCharacter player = hit.collider.GetComponent<BaseCharacter>();

				if (player)
				{
					player.TakeDamage(agent.AttackDamage);
				}
			}
		}

		return SUCCESS;
	}
}
