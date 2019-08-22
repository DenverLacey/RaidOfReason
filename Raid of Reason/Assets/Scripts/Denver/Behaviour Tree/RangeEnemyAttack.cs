using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

public class RangeEnemyAttack : Behaviour
{
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
            // reset attacking variables
            agent.Attacking = false;
			agent.AttackTimer = 0f;

            EnemyProjectile projectile = GameObject.Instantiate(agent.AttackPrefabs[0], agent.transform.position + agent.transform.forward, agent.transform.rotation).GetComponent<EnemyProjectile>();

            if (projectile)
            {
                projectile.Init(agent.AttackDamage, agent);
            }
        }

        return PENDING_COMPOSITE;
    }
}
