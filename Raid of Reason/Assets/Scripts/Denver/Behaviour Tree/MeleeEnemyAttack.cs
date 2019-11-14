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
	private BaseCharacter m_targetPlayer;

	void OnAttackAnimation(EnemyData agent)
	{
		// reset attack timer
		agent.AttackTimer = 0.0f;
		agent.Attacking = false;

		// attack player
		// TODO: Replace with Instantiate(agent.AttackPrefab, agent.transform.position + agent.transform.forward, Quaternion.identity);

		if (m_targetPlayer)
		{
			m_targetPlayer.TakeDamage(agent.AttackDamage);

			//if (m_targetPlayer.tag == "Kreiger" && GameManager.Instance.Kreiger.m_skillUpgrades.Count > 1)
			//{
   //             //if (GameManager.Instance.Kreiger.isActive && GameManager.Instance.Kreiger.m_skillUpgrades.Find(skill => skill.Name == "Static Shield"))
   //             //{
   //             //    agent.TakeDamage(GameManager.Instance.Kreiger.thornDamage, GameManager.Instance.Kreiger);
   //             //}
   //         }
		}
	}

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
		// set OnAttackAnimation delegate for agent
		if (agent.OnAttackDelegate == null)
		{
			agent.OnAttackDelegate = OnAttackAnimation;
		}

		// rotate to face player
		Vector3 direction = (agent.Target - agent.transform.position).normalized;
		direction.y = 0f;
		Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, .25f);

		agent.AttackTimer += Time.deltaTime;

		agent.Pathfinder.StopPathing();

		if (agent.AttackTimer >= agent.AttackCooldown && !agent.Attacking)
		{
			agent.Attacking = true;

			// play attack animation
			agent.SetAnimatorTrigger("Attack");

			if (Physics.Raycast(agent.transform.position, agent.transform.forward, out RaycastHit hit, agent.AttackRange.max))
			{
				m_targetPlayer = hit.collider.GetComponent<BaseCharacter>();
			}
			else
			{
				m_targetPlayer = null;
				return FAILURE;
			}
		}
		return PENDING_COMPOSITE;
	}
}
