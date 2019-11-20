/*
 * Author: Denver
 * Description:	Attack behaviour for the range type enemy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class RangeEnemyAttack : Behaviour
{
	void OnAttackAnimation(EnemyData agent)
	{
		// reset attacking variables
		agent.Attacking = false;
		agent.AttackTimer = 0f;

		EnemyProjectile projectile = GameObject.Instantiate(agent.AttackPrefabs[0], agent.transform.position + agent.transform.forward, agent.transform.rotation).GetComponent<EnemyProjectile>();

		if (projectile)
		{
            AkSoundEngine.PostEvent("Monster_Ranged_Event", projectile.gameObject);

            projectile.Init(agent.AttackDamage, agent);
		}
	}

    /// <summary>
	/// Performs attack behaviour on range enemy agent
	/// </summary>
	/// <param name="agent">
	/// Agent to perform behaviour on
	/// </param>
	/// <returns>
	/// If attack was successfully exectuted
	/// </returns>
    public override Result Execute(EnemyData agent)
    {
		// set OnAttackAnimation delegate for agent
		if (agent.OnAttackDelegate == null)
		{
			agent.OnAttackDelegate = OnAttackAnimation;
		}

		// rotate to face player
        Vector3 direction = (agent.TargetPlayer.transform.position - agent.transform.position).normalized;
		direction.y = 0f;
        Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, .25f);

		// attack player
		agent.Attacking = true;
        agent.AttackTimer += Time.fixedDeltaTime;

        if (agent.AttackTimer >= agent.AttackCooldown)
        {
			//         // reset attacking variables
			//         agent.Attacking = false;
			//agent.AttackTimer = 0f;

			//         EnemyProjectile projectile = GameObject.Instantiate(agent.AttackPrefabs[0], agent.transform.position + agent.transform.forward, agent.transform.rotation).GetComponent<EnemyProjectile>();

			//         if (projectile)
			//         {
			//             projectile.Init(agent.AttackDamage, agent);
			//         }

			agent.SetAnimatorTrigger("Attack");
        }

        return PENDING_COMPOSITE;
    }
}
