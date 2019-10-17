/*
 * Author: Denver
 * Description:	Crystal Suicide Enemy Type Attack Behaviour
 */

using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Crystal Suicide Enemy Type Attack Behaviour
/// </summary>
public class CrystalSuicideAttack : Behaviour
{
	/// <summary>
	/// Performs Crystal Suicide Behaviour on Crystal Suicide Enemies
	/// </summary>
	/// <param name="agent">
	/// agent to perform behaviour on
	/// </param>
	/// <returns>
	/// Result of the behaviour
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
			foreach (var p in GameManager.Instance.AlivePlayers)
			{
				float sqrDistance = (p.transform.position - agent.transform.position).sqrMagnitude;
				if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max)
				{
					p.TakeDamage(agent.AttackDamage);

					if (p.tag == "Kreiger" && p.m_skillUpgrades.Find(skill => skill.Name == "Static Shield"))
					{
						Kreiger krieger = p as Kreiger;
						agent.TakeDamage(krieger.SSDamageTaken, krieger);
					}
				}
			}

			foreach (var crystal in GameObject.FindObjectsOfType<ProtectionObjective>())
			{
				float sqrDistance = (crystal.ProtectObject.transform.position - agent.transform.position).sqrMagnitude;
				if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max)
				{
					crystal.TakeDamage(agent.AttackDamage);
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
