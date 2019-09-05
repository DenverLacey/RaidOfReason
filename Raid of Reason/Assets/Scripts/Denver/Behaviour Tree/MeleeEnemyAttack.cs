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
	/// Performs attack behav     hgdsgszdglkjadfglkjsd g   iour on given melee enemy agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If attack was successfully executed
	/// </returns>
    public override Result Execute(EnemyData agent)
	{
		// rotate to face player
		Vector3 direction = (agent.Target - agent.transform.position).normalized;
		direction.y = 0f;
		Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, .25f);

		agent.Attacking = true;
		agent.AttackTimer += Time.deltaTime;

		agent.Pathfinder.StopPathing();

		if (agent.AttackTimer >= agent.AttackCooldown)
		{
			// reset attacking variables
			agent.Attacking = false;
			agent.AttackTimer = 0.0f;

			// attack player
			// TODO: Replace with Instantiate(agent.AttackPrefab, agent.transform.position + agent.transform.forward, Quaternion.identity);

			// play attack animation
			agent.SetAnimatorTrigger("Attack");

			if (Physics.Raycast(agent.transform.position, agent.transform.forward, out RaycastHit hit, agent.AttackRange.max))
			{
				BaseCharacter player = hit.collider.GetComponent<BaseCharacter>();

				if (player)
				{                    
					player.TakeDamage(agent.AttackDamage);
                    if (player.tag == "Nashorn")
                        agent.isAttackingNashorn = true;

                }
			}
			else
			{
				return FAILURE;
			}
		}

		return PENDING_COMPOSITE;
	}
}
